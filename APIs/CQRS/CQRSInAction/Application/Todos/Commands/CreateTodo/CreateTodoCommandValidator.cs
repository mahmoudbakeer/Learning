using FluentValidation;

namespace CQRSInAction.Application.Todos.Commands.CreateTodo;


public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(ct => ct.Title).NotEmpty().WithMessage("Title cannot be null or empty."); ;

    }
}