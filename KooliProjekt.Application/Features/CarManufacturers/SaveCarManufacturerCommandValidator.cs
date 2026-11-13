using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarManufacturers
{
 public class SaveCarManufacturerCommandValidator : AbstractValidator<SaveCarManufacturerCommand>
 {
  public SaveCarManufacturerCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.CarManufacturers.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
   RuleFor(x => x.Name).MustAsync((x, value, ct) => db.CarManufacturers.AllAsync(e => e.Id == x.Id || e.Name != value, ct)).WithMessage("Value already exists.");
  }
 }
}
