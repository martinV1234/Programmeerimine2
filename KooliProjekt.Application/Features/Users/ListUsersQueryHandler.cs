using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, OperationResult<PagedResult<UserListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<UserListItemDto>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<UserListItemDto>>();
            response.Value = await _dbContext.Users
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new UserListItemDto
                {
                    Id = item.Id,
                    Name = item.Name
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

