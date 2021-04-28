using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project_manager.data.Models
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<UserRole>();
        }

        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }
}
