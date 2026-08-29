using FluentValidation;

namespace CQRSInAction.Application.Todos.Commands.UpdateTodo;


public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(ct => ct.Title).NotEmpty().WithMessage("Title cannot be null or empty.");
        RuleFor(ct => ct.Id).NotEmpty().WithMessage("Id cannot be null or empty.");

    }
}