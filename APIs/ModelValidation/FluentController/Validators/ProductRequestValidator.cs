using ControllerDataAnnotation.Requests;
using FluentValidation;

namespace FluentController.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        // validation logic
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .WithMessage("The Product Name must be more than 3 characters.")
            .MaximumLength(255)
            .WithMessage("The Poduct Name must be less than 255 characters.")
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("The Description can not exceed 1000 charachters.");

        RuleFor(x => x.SKU)
            .NotEmpty()
            .WithMessage("SKU can not be null or empty.")
            .Matches(@"^PRD-\d{5}$")
            .WithMessage("The Product SKU must matches this format 'PRD-XXXXX'.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("The Product Price must be more than 0.0$");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The StockQuantity must be more or equal to 0 pcs.");

        RuleFor(x => x.LaunchDate)
            .Must(IsTodayOrinFuture)
            .WithMessage("The LaunchDate must be today or in the future.");

        RuleFor(x => x.Warranty)
            .Must(IsValidWarrantyRange)
            .WithMessage("The Warrany either be 12 or 24 or 36 months.");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("The Product weight must be more than 0kg.")
            .LessThanOrEqualTo(1000)
            .WithMessage("The Product weight must be less than 1000kg.");

        RuleFor(x => x.Tags)
            .Must(tags => tags.Count <= 5)
            .WithMessage("The Product tags must be maximum 5 tags.");

        RuleFor(x => x.Category).IsInEnum().WithMessage("Category must be valid.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl)
            .When(pr => !string.IsNullOrWhiteSpace(pr.ImageUrl))
            .WithMessage("The ImageUrl must be valid.");

        When(
            x => x.IsReturnable,
            () =>
            {
                RuleFor(x => x.ReturnPolicyDescription)
                    .NotEmpty()
                    .WithMessage(
                        "The ReturnPolicyDescription is needed since the product is returnable."
                    );
            }
        );
    }

    private bool BeValidUrl(string arg) =>
        Uri.TryCreate(arg, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private bool IsValidWarrantyRange(int arg)
    {
        return arg == 12 || arg == 24 || arg == 36;
    }

    private bool IsTodayOrinFuture(DateTime time)
    {
        return time.Date >= DateTime.UtcNow.Date;
    }
}
