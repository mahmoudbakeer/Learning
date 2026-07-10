using System.ComponentModel.DataAnnotations;

namespace ControllerDataAnnotation.Validators
{
    // Custom validation attribute that makes a field required
    // only if another property has a specific value.
    public class RequiredIfAttribute : ValidationAttribute
    {
        // The name of the property to check.
        private readonly string? _Dependentproperty;

        // The value that the dependent property must match.
        private readonly object? _TargetValue;

        // Constructor: stores the property name and target value.
        public RequiredIfAttribute(string? dependentproperty, object? targetObject)
        {
            _Dependentproperty = dependentproperty;
            _TargetValue = targetObject;
        }

        // This method is automatically called during model validation.
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            // Get the type of the model being validated.
            var containertype = context.ObjectInstance.GetType();

            // Find the dependent property by its name using Reflection.
            var field = containertype.GetProperty(_Dependentproperty);

            // If the property doesn't exist, return a validation error.
            if (field is null)
                return new ValidationResult($"Unknown property {_Dependentproperty}");

            // Get the current value of the dependent property.
            var dependentvalue = field.GetValue(context.ObjectInstance, null);

            // If the dependent property's value matches the target value...
            if (Equals(dependentvalue, _TargetValue))
            {
                // ...then the current field becomes required.
                if (value is null || (value is string str && string.IsNullOrEmpty(str)))
                {
                    return new ValidationResult(
                        ErrorMessage ?? $"{context.DisplayName} is Required"
                    );
                }
            }

            // Validation passed.
            return ValidationResult.Success;
        }
    }
}
