using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.CarModels
{
    public class ListCarModelsQuery : IRequest<IEnumerable<CarModel>>
    {
    }
}

