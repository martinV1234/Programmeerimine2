using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Cars
{
 public class SaveCarCommandValidator : AbstractValidator<SaveCarCommand>
 {
  public SaveCarCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.RegistrationNo).NotEmpty().MaximumLength(20);
   RuleFor(x => x.RegistrationNo).MustAsync((x, value, ct) => db.Cars.AllAsync(e => e.Id == x.Id || e.RegistrationNo != value, ct)).WithMessage("Value already exists.");
   RuleFor(x => x.CarModelId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.CarModels.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
  }
 }
}
