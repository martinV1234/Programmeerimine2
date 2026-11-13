using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.InvoiceLines
{
 public class DeleteInvoiceLineCommandValidator : AbstractValidator<DeleteInvoiceLineCommand>
 {
  public DeleteInvoiceLineCommandValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.InvoiceLines.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
