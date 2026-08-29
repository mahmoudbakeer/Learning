using MediatR;

namespace CQRSInAction.Application.Todos.Commands.CreateTodo;


public sealed record CreateTodoCommand(string Title) : IRequest<Guid>; // this will tell the MediatR this is the command of create. i.g adding IRequest<out Response>.