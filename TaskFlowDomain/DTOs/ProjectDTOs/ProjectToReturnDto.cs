using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlowDomain.DTOs.ProjectDTOs
{
    public class ProjectToReturnDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public ICollection<string> Tasks { get; set; } = new List<string>();
    }
}
