using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, Invoice>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Invoice> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        }
    }
}

