using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlowDomain.DTOs.ProjectDTOs
{
    public class UpdateProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
