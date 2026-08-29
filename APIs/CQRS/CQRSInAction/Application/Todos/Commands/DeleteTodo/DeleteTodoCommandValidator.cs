using FluentValidation;

namespace CQRSInAction.Application.Todos.Commands.DeleteTodo;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(ct => ct.Id).NotEmpty().WithMessage("Id cannot be null or empty.");
    }
}

