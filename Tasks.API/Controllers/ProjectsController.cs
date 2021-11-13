using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Results;
using Tasks.Domain._Common.Session;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Dtos.Works;
using Tasks.Domain.Projects.Services;
using Tasks.Domain.Works.Dtos;
using Tasks.Domain.Works.Services;

namespace Tasks.API.Controllers
{
    public class ProjectsController : TasksControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IWorkService _workService;
        private readonly IAutenticationContext _context;

        public ProjectsController(
            IProjectService projectService,
            IWorkService workService,
            IAutenticationContext context
        ) {
            _projectService = projectService;
            _workService = workService;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<Result<ProjectDetailDto>> GetProjectAsync([FromRoute] Guid id)
        {
            return GetResult(await _projectService.GetProjectByIdAsync(id));
        }

        [HttpGet]
        public async Task<Result<IEnumerable<ProjectListDto>>> ListProjectsAsync([FromQuery] PaginationDto paginationDto)
        {
            return GetResult(await _projectService.ListProjectsAsync(paginationDto));
        }

        [HttpGet("works")]
        public async Task<Result<IEnumerable<ProjectWorkListDto>>> ListWorkProjectsAsync([FromQuery] ProjectWorkSearchDto searchDto)
        {
            return GetResult(await _projectService.ListProjectWorksAsync(searchDto));
        }

        [HttpPost]
        public async Task<Result> CreateProjectAsync([FromBody] ProjectCreateDto projectDto)
        {
            return GetResult(await _projectService.CreateProjectAsync(projectDto));
        }

        [HttpPut("{id}")]
        public async Task<Result> UpdateProjectAsync([FromBody] ProjectUpdateDto projectDto, [FromRoute] Guid id)
        {
            projectDto.Id = id;
            return GetResult(await _projectService.UpdateProjectAsync(projectDto));
        }

        [HttpDelete("{id}")]
        public async Task<Result> DeleteProjectAsync([FromRoute] Guid id) 
        { 
            return GetResult(await _projectService.DeleteProjectAsync(id));
        }

        [HttpGet("{id}/works/{workId}")]
        public async Task<Result<ProjectWorkDetailDto>> GetWorkProjectAsync([FromRoute] Guid workId)
        {
            return GetResult(await _projectService.GetProjectWorkByIdAsync(workId));
        }

        [HttpPost("{id}/works")]
        public async Task<Result> CreateWorkProjectAsync([FromBody] WorkClientDto workDto, [FromRoute] Guid id)
        {
            var workCreateDto = new WorkCreateDto(workDto, id, _context.Id);
            return GetResult(await _workService.CreateWorkAsync(workCreateDto));
        }

        [HttpPut("{id}/works/{workId}")]
        public async Task<Result> UpdateWorkProjectAsync([FromBody] WorkClientDto workDto, [FromRoute] Guid workId)
        {
            workDto.Id = workId;
            return GetResult(await _workService.UpdateWorkAsync(new WorkUpdateDto(workDto)));
        }

        [HttpDelete("{id}/works/{workId}")]
        public async Task<Result> DeleteWorkProjectAsync([FromRoute] Guid workId)
        {
            return GetResult(await _workService.DeleteWorkAsync(workId));
        }
    }
}
