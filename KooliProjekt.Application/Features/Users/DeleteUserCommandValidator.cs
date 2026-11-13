using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Users
{
 public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
 {
  public DeleteUserCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Users.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.Bookings.AllAsync(e => e.UserId != id, ct)).WithMessage("Cannot delete a record with related Bookings.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.Invoices.AllAsync(e => e.UserId != id, ct)).WithMessage("Cannot delete a record with related Invoices.");
  }
 }
}
