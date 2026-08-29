using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Queries.GetTodoById;


public sealed record GetTodoByIdQuery(Guid Id) : IRequest<Todo?>;
