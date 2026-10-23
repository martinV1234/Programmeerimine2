using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Features.InvoiceLines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class InvoiceLinesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceLinesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id?}")]
        [ProducesResponseType(typeof(OperationResult<InvoiceLineDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int? id, CancellationToken cancellationToken = default)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }

            var query = new GetInvoiceLineQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<InvoiceLineListItemDto>>), 200)]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new ListInvoiceLinesQuery { Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }

        // Supports the teacher's route format as well as query-string pagination.
        [HttpGet("list/{page:int}/{pageSize:int}")]
        [ProducesResponseType(typeof(OperationResult<PagedResult<InvoiceLineListItemDto>>), 200)]
        public Task<IActionResult> ListPage(int page, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return List(page, pageSize, cancellationToken);
        }
    }
}

