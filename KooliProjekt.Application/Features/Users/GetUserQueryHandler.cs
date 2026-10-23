using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<UserDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<UserDto>();
            response.Value = await _dbContext.Users
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new UserDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
    }
}

