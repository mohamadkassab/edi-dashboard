using System.ComponentModel.DataAnnotations;

namespace EDI.Models.UserModels
{
    public class UserRolePermissionModel
    {
        [Key]
        public int Id { get; set; } 

        [Required, StringLength(100)] 
        public string Role { get; set; }

        public DateTime? Date { get; set; } 

        public bool CanCreateUser { get; set; }

        public bool CanApproveContent { get; set; }

    }
}
