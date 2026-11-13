using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Invoices
{
 public class GetInvoiceQueryValidator : AbstractValidator<GetInvoiceQuery>
 {
  public GetInvoiceQueryValidator(ApplicationDbContext db)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Invoices.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
  }
 }
}
