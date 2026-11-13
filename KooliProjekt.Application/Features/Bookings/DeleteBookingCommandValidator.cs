using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Bookings
{
 public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
 {
  public DeleteBookingCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Bookings.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.InvoiceLines.AllAsync(e => e.BookingId != id, ct)).WithMessage("Cannot delete a record with related InvoiceLines.");
  }
 }
}
