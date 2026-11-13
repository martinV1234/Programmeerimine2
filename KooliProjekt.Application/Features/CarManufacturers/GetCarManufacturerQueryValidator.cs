using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarManufacturers
{
 public class GetCarManufacturerQueryValidator : AbstractValidator<GetCarManufacturerQuery>
 {
  public GetCarManufacturerQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.CarManufacturers.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
