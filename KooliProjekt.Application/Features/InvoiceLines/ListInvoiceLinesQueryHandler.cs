using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class ListInvoiceLinesQueryHandler : IRequestHandler<ListInvoiceLinesQuery, OperationResult<PagedResult<InvoiceLineListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoiceLinesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceLineListItemDto>>> Handle(ListInvoiceLinesQuery request, CancellationToken cancellationToken)
        {
            var response = new OperationResult<PagedResult<InvoiceLineListItemDto>>();
            response.Value = await _dbContext.InvoiceLines
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => new InvoiceLineListItemDto
                {
                    Id = item.Id,
                    LineItem = item.LineItem,
                    Price = item.Price,
                    Unit = item.Unit,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    BookingId = item.BookingId,
                    RegistrationNo = item.Booking.Car.RegistrationNo
                })
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return response;
        }
    }
}

