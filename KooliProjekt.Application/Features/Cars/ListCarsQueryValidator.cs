using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Cars
{
 public class ListCarsQueryValidator : AbstractValidator<ListCarsQuery>
 {
  public ListCarsQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Page).GreaterThan(0);
   RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
  }
 }
}
