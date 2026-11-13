using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.CarManufacturers
{
 public class ListCarManufacturersQueryValidator : AbstractValidator<ListCarManufacturersQuery>
 {
  public ListCarManufacturersQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Page).GreaterThan(0);
   RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
  }
 }
}
