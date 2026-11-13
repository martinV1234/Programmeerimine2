using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Users
{
 public class ListUsersQueryValidator : AbstractValidator<ListUsersQuery>
 {
  public ListUsersQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Page).GreaterThan(0);
   RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
  }
 }
}
