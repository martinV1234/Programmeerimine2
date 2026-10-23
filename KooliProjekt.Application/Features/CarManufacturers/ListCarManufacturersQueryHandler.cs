using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class ListCarManufacturersQueryHandler : IRequestHandler<ListCarManufacturersQuery, OperationResult<PagedResult<CarManufacturerListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCarManufacturersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<CarManufacturerListItemDto>>> Handle(ListCarManufacturersQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<CarManufacturerListItemDto>>();
            response.Value = await _dbContext.CarManufacturers
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new CarManufacturerListItemDto
                {
                    Id = item.Id,
                    Name = item.Name
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

