using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, OperationResult> { private readonly IFileClient _files; public DeleteFileCommandHandler(IFileClient files) { _files = files; } public async Task<OperationResult> Handle(DeleteFileCommand request, CancellationToken ct) { var store = FileStore.Cars + "/" + request.CarId; return await _files.Delete(request.FileName, store, ct) ? new OperationResult() : new OperationResult().AddPropertyError("FileName", "File does not exist."); } } }
