using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
namespace KooliProjekt.Application.Features.Cars { public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand> { public DeleteFileCommandValidator(ApplicationDbContext db) { RuleFor(x => x.CarId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
RuleFor(x => x.FileName).Must(FileRules.ValidName).WithMessage("Invalid file name."); } } }
