using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Bookings
{
    public class ListBookingsQueryHandler : IRequestHandler<ListBookingsQuery, OperationResult<PagedResult<BookingListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBookingsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<BookingListItemDto>>> Handle(ListBookingsQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<BookingListItemDto>>();
            response.Value = await _dbContext.Bookings
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new BookingListItemDto
                {
                    Id = item.Id,
                    Started = item.Started,
                    Finished = item.Finished,
                    RegistrationNo = item.Car.RegistrationNo,
                    ManufacturerName = item.Car.CarModel.CarManufacturer.Name,
                    ModelName = item.Car.CarModel.Name,
                    UserName = item.User.Name
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

