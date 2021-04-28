using project_manager.common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manager.common.Models
{
    public class Project
    {
        public Project()
        {
            Tasks = new List<Task>();
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
        public string ModiffierName { get; set; }
        public ProjectStateEnum ProjectState { get; set; }

        public IList<Task> Tasks { get; set; }
    }
}
