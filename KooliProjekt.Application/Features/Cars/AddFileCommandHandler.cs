using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class AddFileCommandHandler : IRequestHandler<AddFileCommand, OperationResult<string>> { private readonly IFileClient _files; public AddFileCommandHandler(IFileClient files) { _files = files; } public async Task<OperationResult<string>> Handle(AddFileCommand request, CancellationToken ct) { var store = FileStore.Cars + "/" + request.CarId; var name = Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(request.FileName).ToLowerInvariant();
 await _files.Save(name, store, new System.IO.MemoryStream(request.Content, false), ct);
 return new OperationResult<string>(name); } } }

