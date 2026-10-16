using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.TodoItems.Queries.List
{
    public record ListTodoItemsQuery : IRequest<List<TodoItem>>;
}
