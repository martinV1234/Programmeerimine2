using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get(int? id, CancellationToken cancellationToken = default)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }

            var query = new GetCarQuery { Id = id.Value };
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(CancellationToken cancellationToken = default)
        {
            var query = new ListCarsQuery();
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }
    }
}

