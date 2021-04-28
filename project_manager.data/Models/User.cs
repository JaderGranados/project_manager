using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manager.data.Models
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<UserRole>();
            Tasks = new HashSet<UserTask>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string UserName { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Password { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserTask> Tasks { get; set; }
    }
}
