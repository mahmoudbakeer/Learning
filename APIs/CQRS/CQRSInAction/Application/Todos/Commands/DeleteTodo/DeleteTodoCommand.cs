using MediatR;

namespace CQRSInAction.Application.Todos.Commands.DeleteTodo;


public sealed record DeleteTodoCommand(Guid Id) : IRequest; // this will tell the MediaR this is the command of create. i.g adding IRequest<out Response>.