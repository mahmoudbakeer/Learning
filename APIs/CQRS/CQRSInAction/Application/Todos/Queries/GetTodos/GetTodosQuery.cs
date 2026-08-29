using CQRSInAction.Domain.Todos;
using MediatR;

namespace CQRSInAction.Application.Queries.GetTodos;


public sealed record GetTodosQuery : IRequest<List<Todo>?>;
