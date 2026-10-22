using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarModels
{
    public class ListCarModelsQueryHandler : IRequestHandler<ListCarModelsQuery, IEnumerable<CarModel>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCarModelsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CarModel>> Handle(ListCarModelsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.CarModels.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}

