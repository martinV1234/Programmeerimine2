using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class GetFileCommandHandler : IRequestHandler<GetFileCommand, OperationResult<byte[]>> { private readonly IFileClient _files; public GetFileCommandHandler(IFileClient files) { _files = files; } public async Task<OperationResult<byte[]>> Handle(GetFileCommand request, CancellationToken ct) { var store = FileStore.Cars + "/" + request.CarId; return new OperationResult<byte[]>(await _files.Get(request.FileName, store, ct)); } } }
