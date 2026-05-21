using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlowDomain.Entites
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }

        // Navigation Property
        public Project Project { get; set; } = null!;
    }
}
