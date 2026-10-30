using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            if (!await _dbContext.Users.AnyAsync(item => item.Id == request.UserId, cancellationToken))
            {
                return result.AddPropertyError("UserId", "Cannot find related record with ID " + request.UserId);
            }
            var entity = new Invoice();
            if (request.Id == 0)
            {
                _dbContext.Invoices.Add(entity);
            }
            else
            {
                entity = await _dbContext.Invoices.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.InvoiceDate = request.InvoiceDate;
            entity.DueDate = request.DueDate;
            entity.UserId = request.UserId;
            entity.IsPaid = request.IsPaid;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
