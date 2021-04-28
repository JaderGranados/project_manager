using project_manager.common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace project_manager.common.Models
{
    public class Task
    {
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
        public string ModifierBy { get; set; }
        public TaskStateEnum TaskState { get; set; }
    }
}
