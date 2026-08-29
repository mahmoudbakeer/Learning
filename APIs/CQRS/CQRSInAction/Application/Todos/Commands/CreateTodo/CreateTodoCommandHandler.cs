using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Todos.Commands.CreateTodo;



public class CreateTodoCommandHandler(IAppDbContext context) : IRequestHandler<CreateTodoCommand, Guid>
{

    async Task<Guid> IRequestHandler<CreateTodoCommand, Guid>.Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Completed = false
        };

        context.Todos.Add(todo);
        await context.SaveChangesAsync(cancellationToken);
        return todo.Id;
    }
}