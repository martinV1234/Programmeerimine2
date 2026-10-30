using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace KooliProjekt.Application.Features.Bookings
{
    public class SaveBookingCommandHandler : IRequestHandler<SaveBookingCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBookingCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveBookingCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            if (!await _dbContext.Cars.AnyAsync(item => item.Id == request.CarId, cancellationToken))
            {
                return result.AddPropertyError("CarId", "Cannot find related record with ID " + request.CarId);
            }
            if (!await _dbContext.Users.AnyAsync(item => item.Id == request.UserId, cancellationToken))
            {
                return result.AddPropertyError("UserId", "Cannot find related record with ID " + request.UserId);
            }
            var entity = new Booking();
            if (request.Id == 0)
            {
                _dbContext.Bookings.Add(entity);
            }
            else
            {
                entity = await _dbContext.Bookings.SingleOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
                if (entity == null)
                {
                    return result.AddPropertyError("Id", "Cannot find record with ID " + request.Id);
                }
            }
            entity.Started = request.Started;
            entity.Finished = request.Finished;
            entity.StartKm = request.StartKm;
            entity.FinishKm = request.FinishKm;
            entity.KmRate = request.KmRate;
            entity.HourlyRate = request.HourlyRate;
            entity.CarId = request.CarId;
            entity.UserId = request.UserId;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
