using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarModels
{
    public class GetCarModelQueryHandler : IRequestHandler<GetCarModelQuery, OperationResult<CarModelDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCarModelQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<CarModelDto>> Handle(GetCarModelQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<CarModelDto>();
            response.Value = await _dbContext.CarModels
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new CarModelDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    ManufacturerName = item.CarManufacturer.Name,
                    KmRate = item.KmRate,
                    HourlyRate = item.HourlyRate,
                    CarManufacturerId = item.CarManufacturerId,
                    CarCount = item.Cars.Count
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
    }
}

