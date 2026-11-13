using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Bookings
{
 public class ListBookingsQueryValidator : AbstractValidator<ListBookingsQuery>
 {
  public ListBookingsQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Page).GreaterThan(0);
   RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
  }
 }
}
