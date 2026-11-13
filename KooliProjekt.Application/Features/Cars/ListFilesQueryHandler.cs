using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
namespace KooliProjekt.Application.Features.Cars { public class ListFilesQueryHandler : IRequestHandler<ListFilesQuery, OperationResult<System.Collections.Generic.IReadOnlyList<string>>> { private readonly IFileClient _files; public ListFilesQueryHandler(IFileClient files) { _files = files; } public async Task<OperationResult<System.Collections.Generic.IReadOnlyList<string>>> Handle(ListFilesQuery request, CancellationToken ct) { var store = FileStore.Cars + "/" + request.CarId; return new OperationResult<System.Collections.Generic.IReadOnlyList<string>>(await _files.List(store, ct)); } } }
