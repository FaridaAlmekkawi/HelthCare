using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HelthCareProject.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; } // FK

        [Key]
        [Column(Order = 2)]
        public string RoleId { get; set; } // FK

        [ForeignKey("UserId")]
        public virtual user User { get; set; }

        [ForeignKey("RoleId")]
        public virtual role Role { get; set; }
    }
}
