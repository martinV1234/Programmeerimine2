using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class ListCarManufacturersQueryHandler : IRequestHandler<ListCarManufacturersQuery, IEnumerable<CarManufacturer>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListCarManufacturersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CarManufacturer>> Handle(ListCarManufacturersQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.CarManufacturers.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}

