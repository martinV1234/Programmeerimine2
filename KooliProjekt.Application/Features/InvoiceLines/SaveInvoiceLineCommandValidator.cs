using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.InvoiceLines
{
 public class SaveInvoiceLineCommandValidator : AbstractValidator<SaveInvoiceLineCommand>
 {
  public SaveInvoiceLineCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.InvoiceLines.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.LineItem).NotEmpty().MaximumLength(255);
   RuleFor(x => x.Unit).NotEmpty().MaximumLength(20);
   RuleFor(x => x.Price).InclusiveBetween(0m, 100000m).PrecisionScale(10, 2, false);
   RuleFor(x => x.Quantity).InclusiveBetween(0.01m, 100000m).PrecisionScale(10, 2, false);
   RuleFor(x => x.Total).InclusiveBetween(0m, 1000000m).PrecisionScale(12, 2, false);
   RuleFor(x => x.Total).Equal(x => Math.Round(x.Price * x.Quantity, 2, MidpointRounding.AwayFromZero)).WithMessage("Total must equal Price multiplied by Quantity, rounded to two decimals.");
   RuleFor(x => x.BookingId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Bookings.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
  }
 }
}
