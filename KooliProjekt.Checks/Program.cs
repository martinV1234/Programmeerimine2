using System.Reflection;
using System.Text.Json;
using FluentValidation;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Features.Cars;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

if (args.Length == 2 && args[0] == "--cleanup-http") {
 if (!System.Text.RegularExpressions.Regex.IsMatch(args[1], @"^KooliProjekt_HttpFiles_Checks_\d+$")) throw new ArgumentException("Invalid test database name.");
 await using var cleanup = new Microsoft.Data.SqlClient.SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=master;Trusted_Connection=True;TrustServerCertificate=True");
 await cleanup.OpenAsync();
 await using var command = cleanup.CreateCommand();
 command.CommandText = "ALTER DATABASE [" + args[1] + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + args[1] + "];";
 await command.ExecuteNonQueryAsync();
 Console.WriteLine("Removed isolated HTTP test database."); return;
}
var dbName = "KooliProjekt_FileValidation_Checks_" + Guid.NewGuid().ToString("N");
var temp = Path.Combine(Path.GetTempPath(), dbName);
var services = new ServiceCollection();
services.AddLogging();
services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database="+dbName+";Trusted_Connection=True;TrustServerCertificate=True"));
services.AddSingleton<IFileClient>(new LocalFileClient(temp));
var assembly = typeof(AddFileCommand).Assembly;
services.AddValidatorsFromAssembly(assembly);
services.AddMediatR(c => { c.RegisterServicesFromAssembly(assembly); c.AddOpenBehavior(typeof(ValidationBehavior<,>)); c.AddOpenBehavior(typeof(TransactionalBehavior<,>)); });
await using var provider = services.BuildServiceProvider();
await using var scope = provider.CreateAsyncScope();
var db=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
int count=0; var covered=new HashSet<string>();
void Check(bool ok,string label) { if(!ok) throw new Exception("FAIL: "+label); count++; Console.WriteLine("PASS "+label); }
object Request(string name, object values) => JsonSerializer.Deserialize(JsonSerializer.Serialize(values), assembly.GetTypes().Single(t=>t.Name==name))!;
async Task Validate(string name,object request,string property=null) {
 var validator=(IValidator)scope.ServiceProvider.GetRequiredService(typeof(IValidator<>).MakeGenericType(request.GetType()));
 var result=await validator.ValidateAsync(new ValidationContext<object>(request));
 Check(property==null?result.IsValid:result.Errors.Any(e=>e.PropertyName==property),name+" "+(property??"valid")+(result.IsValid?"":" ["+string.Join(",",result.Errors.Select(e=>e.PropertyName))+"]"));
 covered.Add(name);
}
async Task Case(string name,object values,string property=null)=>await Validate(name,Request(name,values),property);
try {
 await db.Database.MigrateAsync();
 var user=new User{Name="Test User",Phone="55555",Email="existing@example.com",Password="pass"};
 var maker=new CarManufacturer{Name="Existing"};db.AddRange(user,maker);await db.SaveChangesAsync();
 var model=new CarModel{Name="Existing",CarManufacturerId=maker.Id,KmRate=1,HourlyRate=2};db.Add(model);await db.SaveChangesAsync();
 var car=new Car{RegistrationNo="EXISTING",CarModelId=model.Id};db.Add(car);await db.SaveChangesAsync();
 var booking=new Booking{CarId=car.Id,UserId=user.Id,Started=new DateTime(2026,1,1),Finished=new DateTime(2026,1,2),StartKm=1,FinishKm=2,KmRate=1,HourlyRate=2};
 var invoice=new Invoice{UserId=user.Id,InvoiceDate=new DateTime(2026,1,1),DueDate=new DateTime(2026,1,2)};db.AddRange(booking,invoice);await db.SaveChangesAsync();
 var line=new InvoiceLine{BookingId=booking.Id,LineItem="Test",Unit="h",Price=1,Quantity=1,Total=1};db.Add(line);db.Entry(line).Property("InvoiceId").CurrentValue=invoice.Id;await db.SaveChangesAsync();
 var examples=new Dictionary<string,Dictionary<string,object>> {
 ["User"]=new(){{"Id",0},{"Name","New User"},{"Phone","55555"},{"Email","new@example.com"},{"Password","pass"}},
 ["CarManufacturer"]=new(){{"Id",0},{"Name","New"}},
 ["CarModel"]=new(){{"Id",0},{"Name","New"},{"CarManufacturerId",maker.Id},{"KmRate",1m},{"HourlyRate",2m}},
 ["Car"]=new(){{"Id",0},{"RegistrationNo","NEW"},{"CarModelId",model.Id}},
 ["Booking"]=new(){{"Id",0},{"Started",new DateTime(2026,1,1)},{"Finished",new DateTime(2026,1,2)},{"StartKm",1m},{"FinishKm",2m},{"KmRate",1m},{"HourlyRate",2m},{"CarId",car.Id},{"UserId",user.Id}},
 ["Invoice"]=new(){{"Id",0},{"InvoiceDate",new DateTime(2026,1,1)},{"DueDate",new DateTime(2026,1,2)},{"UserId",user.Id}},
 ["InvoiceLine"]=new(){{"Id",0},{"LineItem","Test"},{"Unit","h"},{"Price",1m},{"Quantity",1m},{"Total",1m},{"BookingId",booking.Id}}
 };
 var ids=new Dictionary<string,int>{{"User",user.Id},{"CarManufacturer",maker.Id},{"CarModel",model.Id},{"Car",car.Id},{"Booking",booking.Id},{"Invoice",invoice.Id},{"InvoiceLine",line.Id}};
 foreach(var (entity,good) in examples) {
  var name="Save"+entity+"Command";
  await Case(name,good);
  async Task Bad(string property,object value) { var data=new Dictionary<string,object>(good);data[property]=value;await Case(name,data,property); }
  await Bad("Id",-1);await Bad("Id",int.MaxValue);
  var update=new Dictionary<string,object>(good);update["Id"]=ids[entity];await Case(name,update);
  foreach(var (prop,value) in good) {
   if(prop=="Id")continue;
   if(value is string) {
    await Bad(prop,null);await Bad(prop,"");await Bad(prop,"   ");
    int max=prop switch {"Name"=>entity=="User"?25:100,"Phone"=>20,"RegistrationNo"=>20,"Password"=>255,"LineItem"=>255,"Unit"=>20,_=>0};
    if(max>0) {await Bad(prop,new string('x',max+1));var bound=new Dictionary<string,object>(good);bound[prop]=new string('x',max);await Case(name,bound);}
    if(prop=="Name"&&entity=="User")await Bad(prop,"x");
    if(prop=="Phone")await Bad(prop,"1234");
    if(prop=="Email") {await Bad(prop,"bad");await Bad(prop,user.Email);}
    if(entity=="Car"&&prop=="RegistrationNo")await Bad(prop,car.RegistrationNo);
    if(entity=="CarManufacturer"&&prop=="Name")await Bad(prop,maker.Name);
    if(entity=="CarModel"&&prop=="Name")await Bad(prop,model.Name);
   }
   if(prop.EndsWith("Id")){await Bad(prop,0);await Bad(prop,int.MaxValue);}
   if(value is DateTime){await Bad(prop,DateTime.MinValue);if(prop=="Finished"||prop=="DueDate")await Bad(prop,new DateTime(2025,1,1));}
   if(value is decimal) {
    await Bad(prop,-1m);await Bad(prop,0.123m);
    decimal max=prop switch {"KmRate"=>10,"HourlyRate"=>100,"Price"=>100000,"Quantity"=>100000,"Total"=>1000000,_=>99999999.99m};
    await Bad(prop,max+1m);
    if(prop=="KmRate"||prop=="HourlyRate"||prop=="Quantity")await Bad(prop,0m);
    if(prop=="FinishKm")await Bad(prop,0m);
    if(prop=="Total")await Bad(prop,2m);
   }
  }
  var set=entity switch {"CarManufacturer"=>"CarManufacturers","CarModel"=>"CarModels","InvoiceLine"=>"InvoiceLines",_=>entity+"s"};
  await Case("List"+set+"Query",new{Page=1,PageSize=100});
  await Case("List"+set+"Query",new{Page=0,PageSize=10},"Page");
  foreach(var size in new[]{0,101})await Case("List"+set+"Query",new{Page=1,PageSize=size},"PageSize");
  foreach(var op in new[]{"Get","Delete"}) {
   var n=op+entity+(op=="Get"?"Query":"Command");
   await Case(n,new{Id=0},"Id");await Case(n,new{Id=int.MaxValue},"Id");
   if(op=="Get"||entity=="InvoiceLine")await Case(n,new{Id=ids[entity]});
   else await Case(n,new{Id=ids[entity]},"Id");
  }
 }
 // All delete validators also accept an existing owner with no dependent records.
 db.Remove(line);await db.SaveChangesAsync();
 await Case("DeleteBookingCommand",new{Id=booking.Id});await Case("DeleteInvoiceCommand",new{Id=invoice.Id});
 db.RemoveRange(booking,invoice);await db.SaveChangesAsync();await Case("DeleteUserCommand",new{Id=user.Id});await Case("DeleteCarCommand",new{Id=car.Id});
 db.Remove(car);await db.SaveChangesAsync();await Case("DeleteCarModelCommand",new{Id=model.Id});
 db.Remove(model);await db.SaveChangesAsync();await Case("DeleteCarManufacturerCommand",new{Id=maker.Id});
 var model2=new CarModel{Name="File model",CarManufacturerId=maker.Id};db.Add(model2);await db.SaveChangesAsync();
 var cars=new[]{new Car{RegistrationNo="FILE1",CarModelId=model2.Id},new Car{RegistrationNo="FILE2",CarModelId=model2.Id}};db.AddRange(cars);await db.SaveChangesAsync();
 var png=Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+jRZkAAAAASUVORK5CYII=");
 foreach(var name in new[]{"AddFileCommand","DeleteFileCommand","GetFileCommand","ListFilesQuery"}) {
  var data=new Dictionary<string,object>{{"CarId",cars[0].Id},{"FileName","test.png"},{"Content",png}};
  await Case(name,data);
  foreach(var id in new[]{0,int.MaxValue}) {var bad=new Dictionary<string,object>(data);bad["CarId"]=id;await Case(name,bad,"CarId");}
  if(name!="ListFilesQuery")foreach(var filename in new[]{"../escape.png","a/b.png","a\\b.png","CON.png","x.","",new string('x',151)}) {var bad=new Dictionary<string,object>(data);bad["FileName"]=filename;await Case(name,bad,"FileName");}
  if(name=="AddFileCommand")foreach(var content in new byte[][]{null,Array.Empty<byte>(),new byte[FileRules.MaxBytes+1],new byte[20]}) {var bad=new Dictionary<string,object>(data);bad["Content"]=content;await Case(name,bad,"Content");}
 }
 var mediator=scope.ServiceProvider.GetRequiredService<IMediator>();
 var added=await mediator.Send(new AddFileCommand{CarId=cars[0].Id,FileName="test.png",Content=png});Check(!added.HasErrors,"add image");
 var added2=await mediator.Send(new AddFileCommand{CarId=cars[0].Id,FileName="test.png",Content=png});Check(added.Value!=added2.Value,"duplicate names preserved");
 var list=await mediator.Send(new ListFilesQuery{CarId=cars[0].Id});Check(list.Value.Count==2,"list images");
 var read=await mediator.Send(new GetFileCommand{CarId=cars[0].Id,FileName=added.Value});Check(read.Value.SequenceEqual(png),"image bytes round trip");
 var isolated=await mediator.Send(new GetFileCommand{CarId=cars[1].Id,FileName=added.Value});Check(isolated.Value==null,"owner isolation");
 var blocked=await mediator.Send(new DeleteCarCommand{Id=cars[0].Id});Check(blocked.HasErrors,"cannot orphan images");
 var invalid=await mediator.Send(new AddFileCommand{CarId=cars[0].Id,FileName="../escape.png",Content=png});Check(invalid.HasErrors,"pipeline rejects traversal");
 var deleted=await mediator.Send(new DeleteFileCommand{CarId=cars[0].Id,FileName=added.Value});Check(!deleted.HasErrors,"delete image");
 var missing=await mediator.Send(new GetFileCommand{CarId=cars[0].Id,FileName=added.Value});Check(missing.Value==null,"get deleted image");
 var repeated=await mediator.Send(new DeleteFileCommand{CarId=cars[0].Id,FileName=added.Value});Check(repeated.HasErrors,"delete missing image");
 await mediator.Send(new DeleteFileCommand{CarId=cars[0].Id,FileName=added2.Value});
 Check((await mediator.Send(new ListFilesQuery{CarId=cars[0].Id})).Value.Count==0,"empty list");
 var files=scope.ServiceProvider.GetRequiredService<IFileClient>();
 foreach(var store in new[]{"../outside",Path.GetPathRoot(temp)}) {try{await files.List(store);Check(false,"unsafe store");}catch(ArgumentException){Check(true,"unsafe store rejected");}}
 Check(covered.Count==32,"all 32 validators covered");
 Console.WriteLine($"SUCCESS: {count} checks, {covered.Count} validators. Database: {dbName}");
} finally {
 await db.Database.EnsureDeletedAsync();
 if(Directory.Exists(temp))Directory.Delete(temp,true);
}
