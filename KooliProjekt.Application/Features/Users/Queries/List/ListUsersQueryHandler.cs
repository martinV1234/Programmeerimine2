using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users.Queries.List
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<User>>
    {
        private readonly ApplicationDbContext _context;

        public ListUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            return _context.Users.ToListAsync(cancellationToken);
        }
    }
}
