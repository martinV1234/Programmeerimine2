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
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCarCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.Cars.Where(item => item.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}
