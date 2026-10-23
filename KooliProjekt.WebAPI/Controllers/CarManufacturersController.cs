using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Features.CarManufacturers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class CarManufacturersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CarManufacturersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id?}")]
        [ProducesResponseType(typeof(OperationResult<CarManufacturerDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int? id, CancellationToken cancellationToken = default)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }

            var query = new GetCarManufacturerQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<CarManufacturerListItemDto>>), 200)]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new ListCarManufacturersQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        // Supports the teacher's route format as well as query-string pagination.
        [HttpGet("list/{page:int}/{pageSize:int}")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<CarManufacturerListItemDto>>), 200)]
        public Task<IActionResult> ListPage(int page, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return List(page, pageSize, cancellationToken);
        }
    }
}

