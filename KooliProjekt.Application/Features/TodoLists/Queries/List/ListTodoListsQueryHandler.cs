using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TodoLists.Queries.List
{
    public class ListTodoListsQueryHandler : IRequestHandler<ListTodoListsQuery, List<TodoList>>
    {
        private readonly ApplicationDbContext _context;

        public ListTodoListsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<TodoList>> Handle(ListTodoListsQuery request, CancellationToken cancellationToken)
        {
            return _context.TodoLists.ToListAsync(cancellationToken);
        }
    }
}
