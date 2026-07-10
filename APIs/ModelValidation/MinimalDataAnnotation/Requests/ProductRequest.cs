namespace ControllerDataAnnotation.Requests;

using System.ComponentModel.DataAnnotations;
using ControllerDataAnnotation.Enums;
using ControllerDataAnnotation.Validators;

public class ProductRequest
{
    [Required(ErrorMessage = "Product Name is Required")]
    [StringLength(
        255,
        MinimumLength = 3,
        ErrorMessage = "Name must be between 3 and 255 characters"
    )]
    public string? Name { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "SKU is required")]
    [RegularExpression(@"^PRD-\d{5}$", ErrorMessage = "The SKU must follow PRD-XXXXX format")]
    public string? SKU { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "The price must be at least 0.01$")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [CustomValidation(
        validatorType: typeof(LaunchDateValidator),
        method: nameof(LaunchDateValidator.ValidateDate)
    )]
    public DateTime LaunchDate { get; set; }

    [EnumDataType(typeof(ProductCategory), ErrorMessage = "The product Category must be valid.")]
    public ProductCategory Category { get; set; }

    [Url(ErrorMessage = "The ImageUrl is invalid.")]
    public string ImageUrl { get; set; }

    [Range(0.1, 1000, ErrorMessage = "The product weight must be between 0.1kg and 1000kg")]
    public decimal Weight { get; set; }

    [CustomValidation(typeof(WarrantyValidator), nameof(WarrantyValidator.MustBe12_24_36))]
    public int Warranty { get; set; }

    public bool IsReturnable { get; set; }

    [RequiredIf("IsReturnable", true)]
    public string ReturnPolicyDescription { get; set; }

    [MaxLength(5, ErrorMessage = "Maximum 5 tags is allowd")]
    public List<string> Tags { get; set; }
}
