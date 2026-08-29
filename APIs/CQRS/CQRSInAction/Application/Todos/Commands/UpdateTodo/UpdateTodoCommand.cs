using MediatR;

namespace CQRSInAction.Application.Todos.Commands.UpdateTodo;


public sealed record UpdateTodoCommand(Guid Id, string Title, bool Completed) : IRequest; // this will tell the MediatR this is the command of create. i.g adding IRequest<out Response>.