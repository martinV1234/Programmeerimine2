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
    public class DeleteCarModelCommandHandler : IRequestHandler<DeleteCarModelCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCarModelCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCarModelCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.CarModels.Where(item => item.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}
