using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class DeleteCarManufacturerCommandHandler : IRequestHandler<DeleteCarManufacturerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCarManufacturerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCarManufacturerCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.CarManufacturers.Where(item => item.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}
