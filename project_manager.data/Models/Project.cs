using project_manager.common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manager.data.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public ProjectStateEnum ProjectState { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
