using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.CarModels
{
    public class SaveCarModelCommandHandler : IRequestHandler<SaveCarModelCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCarModelCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCarModelCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            if (!await _dbContext.CarManufacturers.AnyAsync(item => item.Id == request.CarManufacturerId, cancellationToken))
            {
                return result.AddPropertyError("CarManufacturerId", "Cannot find related record with ID " + request.CarManufacturerId);
            }
            var entity = new CarModel();
            if (request.Id == 0)
            {
                _dbContext.CarModels.Add(entity);
            }
            else
            {
                entity = await _dbContext.CarModels.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.Name = request.Name;
            entity.CarManufacturerId = request.CarManufacturerId;
            entity.KmRate = request.KmRate;
            entity.HourlyRate = request.HourlyRate;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
