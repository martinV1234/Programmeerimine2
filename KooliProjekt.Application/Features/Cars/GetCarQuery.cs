using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Cars
{
    public class GetCarQuery : IRequest<OperationResult<CarDto>>
    {
        public int Id { get; set; }
    }
}

