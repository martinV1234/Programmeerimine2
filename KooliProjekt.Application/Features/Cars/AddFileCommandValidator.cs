using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
namespace KooliProjekt.Application.Features.Cars { public class AddFileCommandValidator : AbstractValidator<AddFileCommand> { public AddFileCommandValidator(ApplicationDbContext db) { RuleFor(x => x.CarId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist.");
RuleFor(x => x.FileName).Must(FileRules.ValidName).WithMessage("Invalid file name.");
RuleFor(x => x.Content).NotNull().Must(b => b != null && b.Length > 0 && b.Length <= FileRules.MaxBytes).WithMessage("File must contain 1 byte to 5 MB.");
RuleFor(x => x.Content).Must((x, b) => FileRules.IsImage(x.FileName, b)).WithMessage("Only PNG, JPEG and WebP images with matching signatures are allowed."); } } }
