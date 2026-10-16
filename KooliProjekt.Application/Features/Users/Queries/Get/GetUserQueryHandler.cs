using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users.Queries.Get
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly ApplicationDbContext _context;

        public GetUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);
        }
    }
}
