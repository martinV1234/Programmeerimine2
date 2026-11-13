using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Invoices
{
 public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
 {
  public DeleteInvoiceCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Invoices.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.InvoiceLines.AllAsync(e => EF.Property<int?>(e, "InvoiceId") != id, ct)).WithMessage("Cannot delete an invoice with lines.");
  }
 }
}
