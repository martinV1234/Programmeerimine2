using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Cars
{
    public class GetCarQueryHandler : IRequestHandler<GetCarQuery, OperationResult<CarDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCarQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<CarDto>> Handle(GetCarQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<CarDto>();
            response.Value = await _dbContext.Cars
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new CarDto
                {
                    Id = item.Id,
                    RegistrationNo = item.RegistrationNo,
                    ManufacturerName = item.CarModel.CarManufacturer.Name,
                    ModelName = item.CarModel.Name,
                    CarModelId = item.CarModelId,
                    KmRate = item.CarModel.KmRate,
                    HourlyRate = item.CarModel.HourlyRate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
    }
}

