using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 14, MinimumLength = 6, ErrorMessage = "Incorrect password.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
