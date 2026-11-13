using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarModels
{
 public class DeleteCarModelCommandValidator : AbstractValidator<DeleteCarModelCommand>
 {
  public DeleteCarModelCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.CarModels.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.Cars.AllAsync(e => e.CarModelId != id, ct)).WithMessage("Cannot delete a record with related Cars.");
  }
 }
}
