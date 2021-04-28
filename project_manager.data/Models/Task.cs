using project_manager.common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manager.data.Models
{
    public class Task
    {
        public Task()
        {
            Users = new HashSet<UserTask>();
        }

        [Key]
        public int TaskId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime EjecutionDate { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public TaskStateEnum TaskState { get; set; }

        public Project Project { get; set; }
        public ICollection<UserTask> Users { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
