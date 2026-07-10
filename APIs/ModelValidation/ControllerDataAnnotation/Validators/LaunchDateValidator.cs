using System.ComponentModel.DataAnnotations;

namespace ControllerDataAnnotation.Validators;

public class LaunchDateValidator
{
    public static ValidationResult? ValidateDate(DateTime LaunchDate)
    {
        if (LaunchDate.Date >= DateTime.UtcNow.Date)
        {
            return ValidationResult.Success;
        }
        else
            return new ValidationResult("The Date must be today or in the future");
    }
}
