using ControllerProjectManagement.Requests;
using FluentValidation;


namespace ControllerProjectManagement.Validations;

public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
{
    public UpdateBudgetRequestValidator()
    {
        RuleFor(x => x.Budget).GreaterThanOrEqualTo(0);
    }
}

