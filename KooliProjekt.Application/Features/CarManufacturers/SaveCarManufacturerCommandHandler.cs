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
    public class SaveCarManufacturerCommandHandler : IRequestHandler<SaveCarManufacturerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCarManufacturerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCarManufacturerCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var entity = new CarManufacturer();
            if (request.Id == 0)
            {
                _dbContext.CarManufacturers.Add(entity);
            }
            else
            {
                entity = await _dbContext.CarManufacturers.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.Name = request.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
