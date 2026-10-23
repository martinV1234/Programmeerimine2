using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.CarManufacturers
{
    public class GetCarManufacturerQuery : IRequest<OperationResult<CarManufacturerDto>>
    {
        public int Id { get; set; }
    }
}

