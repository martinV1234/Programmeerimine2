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
    public class DeleteInvoiceLineCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
