using CQRSInAction.Application.Common.Exceptions;
using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Queries.GetTodoById;


public class GetTodoByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetTodoByIdQuery, Todo?>
{
    public async Task<Todo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await context.Todos.FindAsync([request.Id], cancellationToken);

        if (todo is null)
            new NotFoundException(nameof(Todo), request.Id); ;
        return todo;
    }
}