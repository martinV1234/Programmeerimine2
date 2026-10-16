using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TodoLists.Queries.Get
{
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, TodoList>
    {
        private readonly ApplicationDbContext _context;

        public GetTodoListQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<TodoList> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            return _context.TodoLists.FirstOrDefaultAsync(todoList => todoList.Id == request.Id, cancellationToken);
        }
    }
}
