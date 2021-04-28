using System.ComponentModel.DataAnnotations;


namespace project_manager.common.Models
{
    public class UserRole
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int RoleId { get; set; }
    }
}
