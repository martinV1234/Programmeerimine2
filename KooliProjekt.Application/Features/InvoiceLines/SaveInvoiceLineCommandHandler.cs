using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class SaveInvoiceLineCommandHandler : IRequestHandler<SaveInvoiceLineCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceLineCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceLineCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            if (!await _dbContext.Bookings.AnyAsync(item => item.Id == request.BookingId, cancellationToken))
            {
                return result.AddPropertyError("BookingId", "Cannot find related record with ID " + request.BookingId);
            }
            var entity = new InvoiceLine();
            if (request.Id == 0)
            {
                _dbContext.InvoiceLines.Add(entity);
            }
            else
            {
                entity = await _dbContext.InvoiceLines.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.LineItem = request.LineItem;
            entity.Price = request.Price;
            entity.Unit = request.Unit;
            entity.Quantity = request.Quantity;
            entity.Total = request.Total;
            entity.BookingId = request.BookingId;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
