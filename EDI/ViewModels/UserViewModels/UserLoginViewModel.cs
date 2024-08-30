using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EDI.ViewModels.UserViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(6, ErrorMessage = "Must be greater than 6 chars")]
        [PasswordPropertyText]
        public string Password { get; set; }

    }
}
