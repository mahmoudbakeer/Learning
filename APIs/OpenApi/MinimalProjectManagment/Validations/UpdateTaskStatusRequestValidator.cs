using ControllerProjectManagement.Requests;
using FluentValidation;


namespace ControllerProjectManagement.Validations;

public class UpdateTaskStatusRequestValidator : AbstractValidator<UpdateTaskStatusRequest>
{
    public UpdateTaskStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

