using System;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
namespace KooliProjekt.Application.Features.Cars { public class ListFilesQueryValidator : AbstractValidator<ListFilesQuery> { public ListFilesQueryValidator(ApplicationDbContext db) { RuleFor(x => x.CarId).Cascade(CascadeMode.Stop).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(e => e.Id == id, ct)).WithMessage("Related record does not exist."); } } }
