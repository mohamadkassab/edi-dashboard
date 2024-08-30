using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDI.Models.UserModels
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Role field is required")]
        public int RoleId { get; set; } 

        [Required, EmailAddress] 
        public string Email { get; set; }

        [Required, StringLength(100)] 
        public string FirstName { get; set; }

        [Required, StringLength(100)] 
        public string LastName { get; set; }

        [Required, StringLength(100)] 
        public string Password { get; set; }

        public DateTime? Date { get; set; } 

        public bool IsActive { get; set; } 



    }
}
