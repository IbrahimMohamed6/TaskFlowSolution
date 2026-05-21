using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlowDomain.DTOs.TaskItemsDTOs
{
    public class TaskItemsToReturnDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Entites.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string Project { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

    }
}
