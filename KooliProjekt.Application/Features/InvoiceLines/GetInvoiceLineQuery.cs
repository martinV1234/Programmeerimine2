using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQuery : IRequest<InvoiceLine>
    {
        public int Id { get; set; }
    }
}

