using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Bookings
{
 public class SaveBookingCommandValidator : AbstractValidator<SaveBookingCommand>
 {
  public SaveBookingCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.Bookings.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Started).NotEmpty();
   RuleFor(x => x.Finished).NotEmpty().GreaterThan(x => x.Started);
   RuleFor(x => x.StartKm).InclusiveBetween(0m, 99999999.99m).PrecisionScale(10, 2, false);
   RuleFor(x => x.FinishKm).GreaterThanOrEqualTo(x => x.StartKm).InclusiveBetween(0m, 99999999.99m).PrecisionScale(10, 2, false);
   RuleFor(x => x.KmRate).InclusiveBetween(0.01m, 10m).PrecisionScale(10, 2, false);
   RuleFor(x => x.HourlyRate).InclusiveBetween(0.01m, 100m).PrecisionScale(10, 2, false);
   RuleFor(x => x.CarId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
   RuleFor(x => x.UserId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Users.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
  }
 }
}
