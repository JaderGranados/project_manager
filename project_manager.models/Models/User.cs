using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manager.common.Models
{
    public class User
    {
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

        public IList<Role> Roles { get; set; }
    }
}
