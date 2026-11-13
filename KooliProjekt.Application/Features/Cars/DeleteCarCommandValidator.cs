using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
namespace KooliProjekt.Application.Features.Cars
{
 public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
 {
  public DeleteCarCommandValidator(ApplicationDbContext db, KooliProjekt.Application.Infrastructure.Files.IFileClient files)
  {
   RuleFor(x => x.Id).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Record does not exist.");
   RuleFor(x => x.Id).MustAsync(async (id, ct) => (await files.List(KooliProjekt.Application.Infrastructure.Files.FileStore.Cars + "/" + id, ct)).Count == 0).WithMessage("Delete the car images first.");
   RuleFor(x => x.Id).MustAsync((id, ct) => db.Bookings.AllAsync(e => e.CarId != id, ct)).WithMessage("Cannot delete a record with related Bookings.");
  }
 }
}
