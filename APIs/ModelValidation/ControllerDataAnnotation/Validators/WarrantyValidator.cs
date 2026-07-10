using System.ComponentModel.DataAnnotations;

namespace ControllerDataAnnotation.Validators;

public class WarrantyValidator
{
    public static ValidationResult? MustBe12_24_36(int value)
    {
        if (value == 12 || value == 24 || value == 36)
        {
            return ValidationResult.Success;
        }
        else
            return new ValidationResult("The warranty must be either 12 or 24 or 36 month.");
    }
}
