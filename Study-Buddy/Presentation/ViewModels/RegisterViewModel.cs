using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Username is too long")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 14, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 14 characters.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password don't match.")]
        public string PasswordConfirmation { get; set; }
    }
}
