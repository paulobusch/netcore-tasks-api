using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Dtos.Works;

namespace Tasks.Domain.Projects.Services
{
    public interface IProjectService {
        Task<Result<ProjectDetailDto>> GetProjectByIdAsync(Guid id);
        Task<Result<ProjectWorkDetailDto>> GetProjectWorkByIdAsync(Guid id);
        Task<Result<IEnumerable<ProjectListDto>>> ListProjectsAsync(PaginationDto paginationDto);
        Task<Result<IEnumerable<ProjectWorkListDto>>> ListProjectWorksAsync(ProjectWorkSearchDto searchDto);
        Task<Result> CreateProjectAsync(ProjectCreateDto projectDto);
        Task<Result> UpdateProjectAsync(ProjectUpdateDto projectDto);
        Task<Result> DeleteProjectAsync(Guid id);
    }
}
