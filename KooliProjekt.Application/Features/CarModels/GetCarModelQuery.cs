using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.CarModels
{
    public class GetCarModelQuery : IRequest<CarModel>
    {
        public int Id { get; set; }
    }
}

