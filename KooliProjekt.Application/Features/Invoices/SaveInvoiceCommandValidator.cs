using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Invoices
{
 public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
 {
  public SaveInvoiceCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThanOrEqualTo(0).MustAsync((id, ct) => id == 0 ? System.Threading.Tasks.Task.FromResult(true) : db.Invoices.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.InvoiceDate).NotEmpty();
   RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(x => x.InvoiceDate);
   RuleFor(x => x.UserId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Users.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
  }
 }
}
