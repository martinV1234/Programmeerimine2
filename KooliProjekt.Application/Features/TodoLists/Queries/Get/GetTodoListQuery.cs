using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.TodoLists.Queries.Get
{
    public record GetTodoListQuery(int Id) : IRequest<TodoList>;
}
