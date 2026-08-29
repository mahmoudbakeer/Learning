using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Todos.Commands.UpdateTodo;



public class UpdateTodoCommandHandler(IAppDbContext context) : IRequestHandler<UpdateTodoCommand>
{

    async Task IRequestHandler<UpdateTodoCommand>.Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await context.Todos.FindAsync([request.Id], cancellationToken);

        if (todo is null)
            throw new KeyNotFoundException($"The Todo with id {request.Id} does not exist.");
        todo.Title = request.Title;
        todo.Completed = request.Completed;

        await context.SaveChangesAsync(cancellationToken);
        await Task.CompletedTask;
    }
}