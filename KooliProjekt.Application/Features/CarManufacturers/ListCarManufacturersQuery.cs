using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class ListCarManufacturersQuery : IRequest<IEnumerable<CarManufacturer>>
    {
    }
}

