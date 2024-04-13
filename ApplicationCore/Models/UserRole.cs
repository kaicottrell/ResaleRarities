using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
}
