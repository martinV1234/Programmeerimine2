using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Users.Queries.List
{
    public record ListUsersQuery : IRequest<List<User>>;
}
