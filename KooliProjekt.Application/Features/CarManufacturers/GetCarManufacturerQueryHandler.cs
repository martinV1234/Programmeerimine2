using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class GetCarManufacturerQueryHandler : IRequestHandler<GetCarManufacturerQuery, CarManufacturer>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCarManufacturerQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CarManufacturer> Handle(GetCarManufacturerQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.CarManufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        }
    }
}

