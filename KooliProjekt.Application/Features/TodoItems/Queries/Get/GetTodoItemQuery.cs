using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.TodoItems.Queries.Get
{
    public record GetTodoItemQuery(int Id) : IRequest<TodoItem>;
}
