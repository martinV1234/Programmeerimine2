using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.TodoLists.Queries.List
{
    public record ListTodoListsQuery : IRequest<List<TodoList>>;
}
