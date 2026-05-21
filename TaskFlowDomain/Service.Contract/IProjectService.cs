using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.DTOs.ProjectDTOs;

namespace TaskFlowDomain.Service.Contract
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectToReturnDto>> GetAllProjects(string userId);

        Task<ProjectToReturnDto?> GetProjectById(Guid id, string userId);

        Task<ProjectToReturnDto> AddProject(CreateProjectDto createProject, string userId);


        Task<ProjectToReturnDto> UpdateProject(UpdateProjectDto updateProject, string userId);

        Task<bool> DeleteProject(Guid id, string userId);
    }
}
