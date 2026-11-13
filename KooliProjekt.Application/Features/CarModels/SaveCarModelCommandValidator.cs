using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarModels
{
 public class SaveCarModelCommandValidator : AbstractValidator<SaveCarModelCommand>
 {
  public SaveCarModelCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.CarModels.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
   RuleFor(x => x.Name).MustAsync((x, value, ct) => db.CarModels.AllAsync(e => e.Id == x.Id || e.Name != value || e.CarManufacturerId != x.CarManufacturerId, ct)).WithMessage("Value already exists.");
   RuleFor(x => x.CarManufacturerId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.CarManufacturers.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
   RuleFor(x => x.KmRate).InclusiveBetween(0.01m, 10m).PrecisionScale(10, 2, false);
   RuleFor(x => x.HourlyRate).InclusiveBetween(0.01m, 100m).PrecisionScale(10, 2, false);
  }
 }
}
