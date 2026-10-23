using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Cars
{
    public class ListCarsQueryHandler : IRequestHandler<ListCarsQuery, OperationResult<PagedResult<CarListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCarsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<CarListItemDto>>> Handle(ListCarsQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<CarListItemDto>>();
            response.Value = await _dbContext.Cars
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new CarListItemDto
                {
                    Id = item.Id,
                    RegistrationNo = item.RegistrationNo,
                    ManufacturerName = item.CarModel.CarManufacturer.Name,
                    ModelName = item.CarModel.Name
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

