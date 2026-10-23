using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Bookings
{
    public class GetBookingQuery : IRequest<OperationResult<BookingDto>>
    {
        public int Id { get; set; }
    }
}

