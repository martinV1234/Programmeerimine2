using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.CarModels
{
    public class GetCarModelQueryHandler : IRequestHandler<GetCarModelQuery, CarModel>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCarModelQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CarModel> Handle(GetCarModelQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.CarModels
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
        }
    }
}

