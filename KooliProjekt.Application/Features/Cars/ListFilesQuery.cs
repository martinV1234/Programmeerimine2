using MediatR;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class ListFilesQuery : IRequest<OperationResult<System.Collections.Generic.IReadOnlyList<string>>> { public int CarId { get; set; }  } }
