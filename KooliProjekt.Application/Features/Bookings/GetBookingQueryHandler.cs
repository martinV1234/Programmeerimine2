using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Bookings
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, OperationResult<BookingDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBookingQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<BookingDto>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<BookingDto>();
            response.Value = await _dbContext.Bookings
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new BookingDto
                {
                    Id = item.Id,
                    Started = item.Started,
                    Finished = item.Finished,
                    RegistrationNo = item.Car.RegistrationNo,
                    ManufacturerName = item.Car.CarModel.CarManufacturer.Name,
                    ModelName = item.Car.CarModel.Name,
                    UserName = item.User.Name,
                    CarId = item.CarId,
                    UserId = item.UserId,
                    StartKm = item.StartKm,
                    FinishKm = item.FinishKm,
                    KmRate = item.KmRate,
                    HourlyRate = item.HourlyRate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
    }
}
