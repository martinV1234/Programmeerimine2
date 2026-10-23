using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQueryHandler : IRequestHandler<GetInvoiceLineQuery, OperationResult<InvoiceLineDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceLineQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceLineDto>> Handle(GetInvoiceLineQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<InvoiceLineDto>();
            response.Value = await _dbContext.InvoiceLines
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new InvoiceLineDto
                {
                    Id = item.Id,
                    LineItem = item.LineItem,
                    Price = item.Price,
                    Unit = item.Unit,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    BookingId = item.BookingId,
                    RegistrationNo = item.Booking.Car.RegistrationNo,
                    Started = item.Booking.Started,
                    Finished = item.Booking.Finished
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }
    }
}

