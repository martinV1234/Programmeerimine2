using System.IO;
using Microsoft.AspNetCore.Http;
using KooliProjekt.Application.Infrastructure.Files;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Features.Cars;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class CarsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id?}")]
        [ProducesResponseType(typeof(OperationResult<CarDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int? id, CancellationToken cancellationToken = default)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }

            var query = new GetCarQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<CarListItemDto>>), 200)]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new ListCarsQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        // Supports the teacher's route format as well as query-string pagination.
        [HttpGet("list/{page:int}/{pageSize:int}")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<CarListItemDto>>), 200)]
        public Task<IActionResult> ListPage(int page, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return List(page, pageSize, cancellationToken);
        }

        [HttpPost("save")]
        [ProducesResponseType(typeof(OperationResult), 200)]
        [ProducesResponseType(typeof(OperationResult), 400)]
        public async Task<IActionResult> Save([FromBody] SaveCarCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Result(result);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(OperationResult), 200)]
        [ProducesResponseType(typeof(OperationResult), 400)]
        public async Task<IActionResult> Delete([FromBody] DeleteCarCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Result(result);
        }

        [HttpPost("{carId:int}/files")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> AddFile(int carId, IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0 || file.Length > FileRules.MaxBytes)
                return Result(new OperationResult().AddPropertyError("File", "A file of 1 byte to 5 MB is required."));
            using var memory = new MemoryStream();
            await file.CopyToAsync(memory, ct);
            return Result(await _mediator.Send(new AddFileCommand { CarId = carId, FileName = file.FileName, Content = memory.ToArray() }, ct));
        }

        [HttpGet("{carId:int}/files")]
        public async Task<IActionResult> ListFiles(int carId, CancellationToken ct) =>
            Result(await _mediator.Send(new ListFilesQuery { CarId = carId }, ct));

        [HttpGet("{carId:int}/files/{fileName}")]
        public async Task<IActionResult> GetFile(int carId, string fileName, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetFileCommand { CarId = carId, FileName = fileName }, ct);
            if (result.HasErrors) return Result((OperationResult)result);
            if (result.Value == null) return NotFound();
            return File(result.Value, FileRules.ContentType(fileName), fileName);
        }

        [HttpDelete("{carId:int}/files/{fileName}")]
        public async Task<IActionResult> DeleteFile(int carId, string fileName, CancellationToken ct) =>
            Result(await _mediator.Send(new DeleteFileCommand { CarId = carId, FileName = fileName }, ct));
    }
}
