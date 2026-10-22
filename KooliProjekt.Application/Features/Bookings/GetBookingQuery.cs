using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Bookings
{
    public class GetBookingQuery : IRequest<Booking>
    {
        public int Id { get; set; }
    }
}

