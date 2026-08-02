using ControllerProjectManagement.Requests;
using FluentValidation;


namespace ControllerProjectManagement.Validations;

public class AssignUserToTaskRequestValidator : AbstractValidator<AssignUserToTaskRequest>
{
    public AssignUserToTaskRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

