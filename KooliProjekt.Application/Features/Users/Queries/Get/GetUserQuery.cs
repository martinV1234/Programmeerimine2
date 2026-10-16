using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Users.Queries.Get
{
    public record GetUserQuery(int Id) : IRequest<User>;
}
