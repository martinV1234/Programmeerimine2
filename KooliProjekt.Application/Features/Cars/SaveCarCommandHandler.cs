using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.Cars
{
    public class SaveCarCommandHandler : IRequestHandler<SaveCarCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCarCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCarCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            if (!await _dbContext.CarModels.AnyAsync(item => item.Id == request.CarModelId, cancellationToken))
            {
                return result.AddPropertyError("CarModelId", "Cannot find related record with ID " + request.CarModelId);
            }
            var entity = new Car();
            if (request.Id == 0)
            {
                _dbContext.Cars.Add(entity);
            }
            else
            {
                entity = await _dbContext.Cars.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.RegistrationNo = request.RegistrationNo;
            entity.CarModelId = request.CarModelId;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
