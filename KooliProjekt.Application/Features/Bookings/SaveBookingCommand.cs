using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Bookings
{
    public class SaveBookingCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public decimal StartKm { get; set; }
        public decimal FinishKm { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
    }
}
