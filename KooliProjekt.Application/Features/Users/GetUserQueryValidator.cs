using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Users
{
 public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
 {
  public GetUserQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Users.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
