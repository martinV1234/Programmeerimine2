using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users.Queries.Get;
using KooliProjekt.Application.Features.Users.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (!id.HasValue || id.Value < 1)
            {
                return NotFound();
            }

            var user = await _mediator.Send(new GetUserQuery(id.Value));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _mediator.Send(new ListUsersQuery());
            return Ok(users);
        }
    }
}
