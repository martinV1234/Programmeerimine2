using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarModels
{
    public class ListCarModelsQueryHandler : IRequestHandler<ListCarModelsQuery, OperationResult<PagedResult<CarModelListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCarModelsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<CarModelListItemDto>>> Handle(ListCarModelsQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<CarModelListItemDto>>();
            response.Value = await _dbContext.CarModels
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new CarModelListItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    ManufacturerName = item.CarManufacturer.Name,
                    KmRate = item.KmRate,
                    HourlyRate = item.HourlyRate
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

