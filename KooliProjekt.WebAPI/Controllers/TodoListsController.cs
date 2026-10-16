using System.Threading.Tasks;
using KooliProjekt.Application.Features.TodoLists.Queries.Get;
using KooliProjekt.Application.Features.TodoLists.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TodoListsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TodoListsController(IMediator mediator)
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

            var todoList = await _mediator.Send(new GetTodoListQuery(id.Value));
            if (todoList == null)
            {
                return NotFound();
            }

            return Ok(todoList);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var todoLists = await _mediator.Send(new ListTodoListsQuery());
            return Ok(todoLists);
        }
    }
}
