using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using CQRSInAction.API.Requests;
using CQRSInAction.Application.Common.Exceptions;
using CQRSInAction.Application.Queries.GetTodoById;
using CQRSInAction.Application.Queries.GetTodos;
using CQRSInAction.Application.Todos.Commands.CreateTodo;
using CQRSInAction.Application.Todos.Commands.DeleteTodo;
using CQRSInAction.Application.Todos.Commands.UpdateTodo;
using CQRSInAction.Domain.Todos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;


namespace CQRSInAction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        return Ok(await mediator.Send(new GetTodosQuery()));
    }

    [HttpGet("{Id:Guid}", Name = "GetTodoById")]
    public async Task<ActionResult<Todo?>> GetTodoById(Guid Id)
    {
        var todo = await mediator.Send(new GetTodoByIdQuery(Id));

        if (todo is null)
            new NotFoundException(nameof(Todo), Id);

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTodo(CreateTodoRequest request)
    {
        var todoId = await mediator.Send(new CreateTodoCommand(request.Title));

        return CreatedAtRoute("GetTodoById", new { Id = todoId }, null);
    }
    [HttpPut("{Id:Guid}")]
    public async Task<ActionResult> UpdateTodo(Guid Id, UpdateTodoRequest request)
    {
        await mediator.Send(new UpdateTodoCommand(Id, request.Title, request.Completed));

        return NoContent();
    }
    [HttpDelete("{Id:Guid}")]
    public async Task<ActionResult> DeleteTodo(Guid Id)
    {
        await mediator.Send(new DeleteTodoCommand(Id));

        return NoContent();
    }
}