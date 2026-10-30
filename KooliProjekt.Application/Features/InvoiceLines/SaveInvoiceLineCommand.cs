using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class SaveInvoiceLineCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string LineItem { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public int BookingId { get; set; }
    }
}
