using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Todos.Commands.DeleteTodo;

public class DeleteTodoCommandHandler(IAppDbContext context) : IRequestHandler<DeleteTodoCommand>
{
    async Task IRequestHandler<DeleteTodoCommand>.Handle(
        DeleteTodoCommand request,
        CancellationToken cancellationToken
    )
    {
        var todo = await context.Todos.FindAsync([request.Id], cancellationToken);

        if (todo is null)
            throw new KeyNotFoundException($"The Todo with id {request.Id} does not exist.");
        context.Todos.Remove(todo);

        await context.SaveChangesAsync(cancellationToken);
        await Task.CompletedTask;
    }
}

