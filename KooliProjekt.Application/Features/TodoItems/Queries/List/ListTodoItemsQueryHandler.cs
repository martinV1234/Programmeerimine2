using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TodoItems.Queries.List
{
    public class ListTodoItemsQueryHandler : IRequestHandler<ListTodoItemsQuery, List<TodoItem>>
    {
        private readonly ApplicationDbContext _context;

        public ListTodoItemsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<TodoItem>> Handle(ListTodoItemsQuery request, CancellationToken cancellationToken)
        {
            return _context.TodoItems.ToListAsync(cancellationToken);
        }
    }
}
