using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Dtos.Works;
using Tasks.Domain.Projects.Entities;
using Tasks.Domain.Projects.Repositories;
using Tasks.Domain.Projects.Services;
using Tasks.Domain.Works.Repositories;

namespace Tasks.Service.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkRepository _workRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IWorkRepository workRepository
        ) {
            _projectRepository = projectRepository;
            _workRepository = workRepository;
        }

        public async Task<Result> CreateProjectAsync(ProjectCreateDto projectDto)
        {
            var existTitle = await _projectRepository.ExistByTitleAsync(projectDto.Title);
            if (existTitle) return new Result(Status.Conflict, $"Project with {nameof(projectDto.Title)} already exist");

            var projectId = projectDto.Id == Guid.Empty ? Guid.NewGuid() : projectDto.Id;
            var project = new Project(
                id: projectId,
                title: projectDto.Title,
                description: projectDto.Description,
                developerProjects: GetDeveloperProjects(projectDto.DeveloperIds, projectId)
            );

            await _projectRepository.CreateAsync(project);
            return new Result();
        }

        public async Task<Result> DeleteProjectAsync(Guid id)
        {
            var existProject = await _projectRepository.ExistAsync(id);
            if (!existProject) return new Result(Status.NotFund, $"Project with {nameof(id)} does not exist");

            var project = await _projectRepository.GetByIdAsync(id);
            await _projectRepository.DeleteAsync(project);
            return new Result();
        }

        public async Task<Result<ProjectDetailDto>> GetProjectByIdAsync(Guid id)
        {
            var existProject = await _projectRepository.ExistAsync(id);
            if (!existProject) return new Result<ProjectDetailDto>(Status.NotFund, $"Project with {nameof(id)} does not exist");

            var project = await _projectRepository.Query()
                .Include(p => p.DeveloperProjects)
                    .ThenInclude(dp => dp.Developer)
                .SingleAsync(p => p.Id == id);
            var projectDetail = new ProjectDetailDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Developers = project.DeveloperProjects.Select(dp => new DeveloperListDto { 
                    Id = dp.DeveloperId,
                    Name = dp.Developer.Name
                })
            };

            return new Result<ProjectDetailDto>(projectDetail);
        }

        public async Task<Result<ProjectWorkDetailDto>> GetProjectWorkByIdAsync(Guid id)
        {
            var existWork = await _workRepository.ExistAsync(id);
            if (!existWork) return new Result<ProjectWorkDetailDto>(Status.NotFund, $"Work with {nameof(id)} does not exist");

            var work = await _workRepository.Query()
                .Include(w => w.DeveloperProject)
                    .ThenInclude(dp => dp.Project)
                .Include(w => w.DeveloperProject)
                    .ThenInclude(dp => dp.Developer)
                .SingleAsync(p => p.Id == id);

            var workDetail = new ProjectWorkDetailDto
            {
                Id = work.Id,
                StartTime = work.StartTime,
                EndTime = work.EndTime,
                Comment = work.Comment,
                Hours = work.Hours,
                Project = new ProjectListDto
                {
                    Id = work.DeveloperProject.ProjectId,
                    Title = work.DeveloperProject.Project.Title
                },
                Developer = new DeveloperListDto
                {
                    Id = work.DeveloperProject.DeveloperId,
                    Name = work.DeveloperProject.Developer.Name
                }
            };

            return new Result<ProjectWorkDetailDto>(workDetail);
        }

        public async Task<Result<IEnumerable<ProjectListDto>>> ListProjectsAsync(PaginationDto pagination)
        {
            var query = _projectRepository.Query();
            var projects = await query
                .Skip(pagination.CalculateOffset())
                .Take(pagination.Limit)
                .Select(d => new ProjectListDto
                {
                    Id = d.Id,
                    Title = d.Title
                })
                .ToArrayAsync();

            return new Result<IEnumerable<ProjectListDto>>(projects, await query.CountAsync());
        }

        public async Task<Result<IEnumerable<ProjectWorkListDto>>> ListProjectWorksAsync(ProjectWorkSearchDto searchDto)
        {
            var query = _workRepository.Query()
                .Where(w => searchDto.ProjectId == null || w.DeveloperProject.ProjectId == searchDto.ProjectId)
                .Where(w => searchDto.DeveloperId == null || w.DeveloperProject.DeveloperId == searchDto.DeveloperId);
            
            var projectWorks = await query
                .Skip(searchDto.CalculateOffset())
                .Take(searchDto.Limit)
                .Select(w => new ProjectWorkListDto
                {
                    Id = w.Id,
                    StartTime = w.StartTime,
                    EndTime = w.EndTime,
                    Comment = w.Comment,
                    Hours = w.Hours,
                    ProjectId = w.DeveloperProject.ProjectId,
                    ProjectTitle = w.DeveloperProject.Project.Title,
                    DeveloperId = w.DeveloperProject.DeveloperId,
                    DeveloperName = w.DeveloperProject.Developer.Name
                })
                .ToArrayAsync();

            return new Result<IEnumerable<ProjectWorkListDto>>(projectWorks, await query.CountAsync());
        }

        public async Task<Result> UpdateProjectAsync(ProjectUpdateDto projectDto)
        {
            var existProject = await _projectRepository.ExistAsync(projectDto.Id);
            if (!existProject) return new Result(Status.NotFund, $"Project with {nameof(projectDto.Id)} does not exist");
            var existTitle = await _projectRepository.ExistByTitleAsync(projectDto.Title, projectDto.Id);
            if (existTitle) return new Result(Status.Conflict, $"Project with {nameof(projectDto.Title)} already exist");

            var project = await _projectRepository.Query()
                .Include(p => p.DeveloperProjects)
                .FirstOrDefaultAsync(p => p.Id == projectDto.Id);
            project.SetData(
                title: projectDto.Title,
                description: projectDto.Description,
                developerProjects: GetDeveloperProjects(projectDto.DeveloperIds, project.Id)
            );

            await _projectRepository.UpdateAsync(project);
            return new Result();
        }

        private IEnumerable<DeveloperProject> GetDeveloperProjects(IEnumerable<Guid> developerIds, Guid projectId)
        {
            var develperProjects = new List<DeveloperProject>();
            if (developerIds == null || !developerIds.Any()) return develperProjects;
            foreach (var developerId in developerIds)
            {
                var developerProject = new DeveloperProject(
                    id: default,
                    developerId: developerId,
                    projectId: projectId
                );

                develperProjects.Add(developerProject);
            }
            return develperProjects;
        }
    }
}
