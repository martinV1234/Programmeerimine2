using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Cars
{
 public class GetCarQueryValidator : AbstractValidator<GetCarQuery>
 {
  public GetCarQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
