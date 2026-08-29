using CQRSInAction.Application.Common.Interfaces;
using CQRSInAction.Application.Queries.GetTodos;
using CQRSInAction.Domain.Todos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSInAction.Application.Queries.GetTodoById;


public class GetTodosQueryHandler(IAppDbContext context) : IRequestHandler<GetTodosQuery, List<Todo>?>
{


    public async Task<List<Todo>?> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return await context.Todos.ToListAsync(cancellationToken);
    }
}