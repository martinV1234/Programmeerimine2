using MediatR;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class AddFileCommand : IRequest<OperationResult<string>> { public int CarId { get; set; } public string FileName { get; set; }
 public byte[] Content { get; set; } } }
