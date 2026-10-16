using System.Threading.Tasks;
using KooliProjekt.Application.Features.TodoItems.Queries.Get;
using KooliProjekt.Application.Features.TodoItems.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TodoItemsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
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

            var todoItem = await _mediator.Send(new GetTodoItemQuery(id.Value));
            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var todoItems = await _mediator.Send(new ListTodoItemsQuery());
            return Ok(todoItems);
        }
    }
}
