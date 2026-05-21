using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlowDomain.DTOs.TaskItemsDTOs
{
    public class UpdateTaskStatuesDto
    {
        public Entites.TaskStatus Status { get; set; }
    }
}
