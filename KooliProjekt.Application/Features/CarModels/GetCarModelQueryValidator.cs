using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarModels
{
 public class GetCarModelQueryValidator : AbstractValidator<GetCarModelQuery>
 {
  public GetCarModelQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.CarModels.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
