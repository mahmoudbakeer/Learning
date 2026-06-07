using System.ComponentModel.DataAnnotations;

namespace ModelValidation
{
    public class Registeration
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string? Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6 , ErrorMessage = "the password must be atleast 6 character, and max 100")]
        public string? Password { get; set; }
        [Required (ErrorMessage = "Passowrd confirmation is required.")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string? Confirmation {  get; set; }

       public static ValueTask<Registeration> BindAsync(HttpContext context)
        {
            string? Email = context.Request.Query["email"];
            string? Password = context.Request.Query["pwd1"];
            string? Confirmation = context.Request.Query["pwd2"];

            return
                new ValueTask<Registeration>(new Registeration { Email = Email, Password = Password, Confirmation = Confirmation });
        }
    }
}
