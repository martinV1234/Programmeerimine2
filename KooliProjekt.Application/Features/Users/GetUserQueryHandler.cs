using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        }
    }
}

