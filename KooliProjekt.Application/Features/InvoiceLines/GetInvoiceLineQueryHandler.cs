using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQueryHandler : IRequestHandler<GetInvoiceLineQuery, InvoiceLine>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceLineQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InvoiceLine> Handle(GetInvoiceLineQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.InvoiceLines
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        }
    }
}

