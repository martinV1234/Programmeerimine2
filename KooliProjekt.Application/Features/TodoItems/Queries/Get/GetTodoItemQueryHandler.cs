using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TodoItems.Queries.Get
{
    public class GetTodoItemQueryHandler : IRequestHandler<GetTodoItemQuery, TodoItem>
    {
        private readonly ApplicationDbContext _context;

        public GetTodoItemQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<TodoItem> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
        {
            return _context.TodoItems.FirstOrDefaultAsync(todoItem => todoItem.Id == request.Id, cancellationToken);
        }
    }
}
