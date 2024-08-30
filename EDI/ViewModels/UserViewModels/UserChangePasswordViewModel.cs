using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EDI.ViewModels.UserViewModels
{
    public class UserChangePasswordViewModel
    {
        [Required]
        [MaxLength(255)]
        [MinLength(6, ErrorMessage = "Must be greater than 6 chars")]
        [PasswordPropertyText]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(6, ErrorMessage = "Must be greater than 6 chars")]
        [PasswordPropertyText]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(6, ErrorMessage = "Must be greater than 6 chars")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [PasswordPropertyText]
        public string ConfirmNewPassword { get; set; }
    }
}
