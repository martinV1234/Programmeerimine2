using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Authentication;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace KooliProjekt.Application.Features.Public;
public record ProfileDto(int Id,string Name,string Phone,string Email);
public record CarInfo(int Id,string RegistrationNo,string Manufacturer,string Model,decimal KmRate,decimal HourlyRate);
public record RentalInfo(int Id,int CarId,string RegistrationNo,DateTime Started,DateTime Finished,bool IsActive,decimal StartKm,decimal FinishKm,decimal KmRate,decimal HourlyRate);
public record InvoiceLineInfo(int Id,string LineItem,decimal Price,string Unit,decimal Quantity,decimal Total);
public record InvoiceInfo(int Id,DateTime InvoiceDate,DateTime DueDate,bool IsPaid,List<InvoiceLineInfo> Lines);
public record MyProfileQuery : IRequest<OperationResult<ProfileDto>>;
public record UpdateMyProfileCommand(string Name,string Phone,string Email) : IRequest<OperationResult>;
public record AvailableCarsQuery(DateTime From,DateTime Until) : IRequest<OperationResult<List<CarInfo>>>;
public record MyRentalsQuery : IRequest<OperationResult<List<RentalInfo>>>;
public record MyRentalQuery(int Id) : IRequest<OperationResult<RentalInfo>>;
public record RentCarCommand(int CarId,DateTime Until,decimal StartKm) : IRequest<OperationResult<int>>;
public record FinishMyRentalCommand(int Id,decimal FinishKm) : IRequest<OperationResult>;
public record MyInvoicesQuery : IRequest<OperationResult<List<InvoiceInfo>>>;
public record MyInvoiceQuery(int Id) : IRequest<OperationResult<InvoiceInfo>>;
public record RegisterCommand(string Name,string Phone,string Email,string Password) : IRequest<OperationResult>;
public class UpdateMyProfileValidator : AbstractValidator<UpdateMyProfileCommand> {
 public UpdateMyProfileValidator() { RuleFor(x=>x.Name).NotEmpty().Length(2,25); RuleFor(x=>x.Phone).NotEmpty().Length(5,20); RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(254); }
}
public class RegisterValidator : AbstractValidator<RegisterCommand> {
 public RegisterValidator() { RuleFor(x=>x.Name).NotEmpty().Length(2,25); RuleFor(x=>x.Phone).NotEmpty().Length(5,20); RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(254); RuleFor(x=>x.Password).NotEmpty().MinimumLength(8).MaximumLength(255); }
}
public class PublicHandler : IRequestHandler<MyProfileQuery,OperationResult<ProfileDto>>, IRequestHandler<UpdateMyProfileCommand,OperationResult>, IRequestHandler<AvailableCarsQuery,OperationResult<List<CarInfo>>>, IRequestHandler<MyRentalsQuery,OperationResult<List<RentalInfo>>>, IRequestHandler<MyRentalQuery,OperationResult<RentalInfo>>, IRequestHandler<RentCarCommand,OperationResult<int>>, IRequestHandler<FinishMyRentalCommand,OperationResult>, IRequestHandler<MyInvoicesQuery,OperationResult<List<InvoiceInfo>>>, IRequestHandler<MyInvoiceQuery,OperationResult<InvoiceInfo>>, IRequestHandler<RegisterCommand,OperationResult> {
 readonly ApplicationDbContext db; readonly IUserProvider user;
 public PublicHandler(ApplicationDbContext db,IUserProvider user) { this.db=db; this.user=user; }
 int UserId => user.UserId() ?? 0;
 IQueryable<RentalInfo> Rentals(int? id = null) => db.Bookings.AsNoTracking().Where(b=>b.UserId==UserId && (!id.HasValue || b.Id==id.Value)).OrderByDescending(b=>b.Started).Select(b=>new RentalInfo(b.Id,b.CarId,b.Car.RegistrationNo,b.Started,b.Finished,b.IsActive,b.StartKm,b.FinishKm,b.KmRate,b.HourlyRate));
 IQueryable<InvoiceInfo> Invoices(int? id = null) => db.Invoices.AsNoTracking().Where(i=>i.UserId==UserId && (!id.HasValue || i.Id==id.Value)).OrderByDescending(i=>i.InvoiceDate).Select(i=>new InvoiceInfo(i.Id,i.InvoiceDate,i.DueDate,i.IsPaid,i.InvoiceLines.Where(l=>l.Booking.UserId==UserId).Select(l=>new InvoiceLineInfo(l.Id,l.LineItem,l.Price,l.Unit,l.Quantity,l.Total)).ToList()));
 public async Task<OperationResult<ProfileDto>> Handle(MyProfileQuery q,CancellationToken ct) => new(await db.Users.AsNoTracking().Where(u=>u.Id==UserId).Select(u=>new ProfileDto(u.Id,u.Name,u.Phone,u.Email)).FirstOrDefaultAsync(ct));
 public async Task<OperationResult> Handle(UpdateMyProfileCommand q,CancellationToken ct) {
  var r=new OperationResult(); var u=await db.Users.FirstOrDefaultAsync(x=>x.Id==UserId,ct); if(u==null)return r.AddError("Kasutajat ei leitud.");
  var email=q.Email.Trim(); if(await db.Users.AnyAsync(x=>x.Id!=UserId && x.Email==email,ct))return r.AddError("E-posti aadress on juba kasutusel.");
  u.Name=q.Name.Trim();u.Phone=q.Phone.Trim();u.Email=email;await db.SaveChangesAsync(ct);return r;
 }
 public async Task<OperationResult> Handle(RegisterCommand q,CancellationToken ct) {
  var r=new OperationResult();var email=q.Email.Trim();if(await db.Users.AnyAsync(x=>x.Email==email,ct))return r.AddError("E-posti aadress on juba kasutusel.");
  db.Users.Add(new User{Name=q.Name.Trim(),Phone=q.Phone.Trim(),Email=email,Password=PasswordHelper.Hash(q.Password),IsAdmin=false});await db.SaveChangesAsync(ct);return r;
 }
 public async Task<OperationResult<List<CarInfo>>> Handle(AvailableCarsQuery q,CancellationToken ct) {
  if(q.From==default || q.Until<=q.From || q.From<DateTime.UtcNow.AddMinutes(-1))return new OperationResult<List<CarInfo>>().AddError("Vali kehtiv tulevane rendiperiood (UTC).");
  var cars=await db.Cars.AsNoTracking().Where(c=>!db.Bookings.Any(b=>b.CarId==c.Id && (b.IsActive || (b.Started<q.Until && b.Finished>q.From)))).OrderBy(c=>c.Id).Select(c=>new CarInfo(c.Id,c.RegistrationNo,c.CarModel.CarManufacturer.Name,c.CarModel.Name,c.CarModel.KmRate,c.CarModel.HourlyRate)).ToListAsync(ct);return new(cars);
 }
 public async Task<OperationResult<List<RentalInfo>>> Handle(MyRentalsQuery q,CancellationToken ct)=>new(await Rentals().ToListAsync(ct));
 public async Task<OperationResult<RentalInfo>> Handle(MyRentalQuery q,CancellationToken ct)=>new(await Rentals(q.Id).FirstOrDefaultAsync(ct));
 public async Task<OperationResult<List<InvoiceInfo>>> Handle(MyInvoicesQuery q,CancellationToken ct)=>new(await Invoices().ToListAsync(ct));
 public async Task<OperationResult<InvoiceInfo>> Handle(MyInvoiceQuery q,CancellationToken ct)=>new(await Invoices(q.Id).FirstOrDefaultAsync(ct));
 public async Task<OperationResult<int>> Handle(RentCarCommand q,CancellationToken ct) {
  var r=new OperationResult<int>();var now=DateTime.UtcNow;
  if(q.Until<=now || q.StartKm<0 || q.StartKm>99999999.99m || decimal.Round(q.StartKm,2)!=q.StartKm)return r.AddError("Kontrolli lõppaega ja algset läbisõitu.");
  await using var tx=await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable,ct);
  if(!await db.Users.AnyAsync(u=>u.Id==UserId,ct))return r.AddError("Kasutajat ei leitud.");
  var car=await db.Cars.Include(c=>c.CarModel).FirstOrDefaultAsync(c=>c.Id==q.CarId,ct);if(car==null)return r.AddError("Autot ei leitud.");
  if(await db.Bookings.AnyAsync(b=>(b.IsActive || (b.Started<q.Until && b.Finished>now)) && (b.CarId==q.CarId || b.UserId==UserId),ct))return r.AddError("Auto või kasutaja on sel ajal juba rendiga seotud.");
  var b=new Booking{CarId=car.Id,UserId=UserId,Started=now,Finished=q.Until,IsActive=true,StartKm=q.StartKm,FinishKm=q.StartKm,KmRate=car.CarModel.KmRate,HourlyRate=car.CarModel.HourlyRate};db.Bookings.Add(b);await db.SaveChangesAsync(ct);await tx.CommitAsync(ct);r.Value=b.Id;return r;
 }
 public async Task<OperationResult> Handle(FinishMyRentalCommand q,CancellationToken ct) {
  var r=new OperationResult();
  await using var tx=await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable,ct);
  var b=await db.Bookings.FirstOrDefaultAsync(x=>x.Id==q.Id && x.UserId==UserId,ct);
  if(b==null)return r.AddError("Renti ei leitud.");
  var now=DateTime.UtcNow;
  if(!b.IsActive || b.Started>now)return r.AddError("See rent ei ole käimas.");
  var distance=q.FinishKm-b.StartKm;
  if(distance<0 || q.FinishKm>99999999.99m || decimal.Round(q.FinishKm,2)!=q.FinishKm || distance>100000m)
   return r.AddError("Kontrolli lõppläbisõitu; see peab olema algsest suurem või võrdne ning läbitud vahemaa kuni 100 000 km.");
  // Bill each started hundredth of an hour; both quantities use two decimal places.
  var hours=Math.Max(0.01m,Math.Ceiling((decimal)(now-b.Started).TotalHours*100m)/100m);
  var timeTotal=Math.Round(hours*b.HourlyRate,2,MidpointRounding.AwayFromZero);
  var distanceTotal=Math.Round(distance*b.KmRate,2,MidpointRounding.AwayFromZero);
  if(hours>100000m || timeTotal>1000000m || distanceTotal>1000000m)
   return r.AddError("Rendi arvestus ületab lubatud piiri. Pöördu administraatori poole.");
  b.Finished=now;b.FinishKm=q.FinishKm;b.IsActive=false;
  var invoice=new Invoice { UserId=UserId,InvoiceDate=now,DueDate=now.AddDays(14),IsPaid=false };
  invoice.InvoiceLines.Add(new InvoiceLine { BookingId=b.Id,LineItem="Autorendi aeg",Unit="h",Price=b.HourlyRate,Quantity=hours,Total=timeTotal });
  if(distance>0)invoice.InvoiceLines.Add(new InvoiceLine { BookingId=b.Id,LineItem="Autorendi läbisõit",Unit="km",Price=b.KmRate,Quantity=distance,Total=distanceTotal });
  db.Invoices.Add(invoice);
  await db.SaveChangesAsync(ct);await tx.CommitAsync(ct);return r;
 }
}
