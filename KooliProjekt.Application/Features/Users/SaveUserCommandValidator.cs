using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Users
{
 public class SaveUserCommandValidator : AbstractValidator<SaveUserCommand>
 {
  public SaveUserCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.Users.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Name).NotEmpty().Length(2, 25);
   RuleFor(x => x.Phone).NotEmpty().Length(5, 20);
   RuleFor(x => x.Email).NotEmpty().EmailAddress();
   RuleFor(x => x.Email).MustAsync((x, value, ct) => db.Users.AllAsync(e => e.Id == x.Id || e.Email != value, ct)).WithMessage("Value already exists.");
   RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
  }
 }
}
