using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Projects.Dtos;
using Tasks.Domain.Projects.Dtos.Works;
using System.Threading.Tasks;
using Xunit;
using Tasks.Domain.Developers.Entities;

namespace Tasks.API.IntegrationTests.Controllers
{
    public class ProjectsControllerTests : TestBase
    {
        public ProjectsControllerTests(TasksAPIFixture fixture) : base(fixture, "api/projects") { }
        
        [Fact]
        public async Task ShouldReturnByIdAsync()
        {
            var project = EntityFactory.Project().Save();
        
            var (response, result) = await Request.GetAsync<Result<ProjectDetailDto>>(new Uri($"{Uri}/{project.Id}"));
        
            response.EnsureSuccessStatusCode();
            var modelResponse = result?.Data;
            Assert.NotEqual(default, modelResponse);
            CompareFactory.ProjectEqualsProjectDetailDto().Equals(project, modelResponse);
        }
        
        [Fact]
        public async Task ShouldReturnManyAsync()
        {
            var project = EntityFactory.Project().Save();
            var paginationDtoRequest = ModelFactory.PaginationDto().Generate();
        
            var (response, result) = await Request.GetAsync<Result<IEnumerable<ProjectListDto>>>(Uri, paginationDtoRequest);
        
            response.EnsureSuccessStatusCode();
            var listResponse = result?.Data;
            Assert.NotEqual(default, listResponse);
            Assert.NotEmpty(listResponse);
            var modelResponse = listResponse.Single(p => p.Id == project.Id);
            CompareFactory.ProjectEqualsProjectListDto().Equals(project, modelResponse);
        }
        
        [Fact]
        public async Task ShouldReturnManyAsync1()
        {
            var project = EntityFactory.Project().Save();
            var projectWorkSearchDtoRequest = ModelFactory.ProjectWorkSearchDto().Generate();
        
            var (response, result) = await Request.GetAsync<Result<IEnumerable<ProjectWorkListDto>>>(new Uri($"{Uri}/works"), projectWorkSearchDtoRequest);
        
            response.EnsureSuccessStatusCode();
            var listResponse = result?.Data;
            Assert.NotEqual(default, listResponse);
            Assert.NotEmpty(listResponse);
        }
        
        [Fact]
        public async Task ShouldReturnByIdAsync1()
        {
            var project = EntityFactory.Project().Save();
            var work = project.DeveloperProjects.Single().Works.Single();
        
            var (response, result) = await Request.GetAsync<Result<ProjectWorkDetailDto>>(new Uri($"{Uri}/{project.Id}/works/{work.Id}"));
        
            response.EnsureSuccessStatusCode();
            var modelResponse = result?.Data;
            Assert.NotEqual(default, modelResponse);
        }
        
        [Fact]
        public async Task ShouldCreateProjectAsync()
        {
            var projectCreateDtoRequest = ModelFactory.ProjectCreateDto().Generate();
        
            var (response, result) = await Request.PostAsync<Result>(Uri, projectCreateDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
              
            var project = await DbContext.Projects.FindAsync(projectCreateDtoRequest.Id);
            CompareFactory.ProjectEqualsProjectCreateDto().Equals(projectCreateDtoRequest, project);
        }
        
        [Fact]
        public async Task ShouldCreateWorkProjectAsync()
        {
            var project = EntityFactory.Project().Save();
            var workClientDtoRequest = ModelFactory.WorkClientDto().Generate();
        
            var (response, result) = await Request.PostAsync<Result>(new Uri($"{Uri}/{project.Id}/works"), workClientDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
        }
        
        [Fact]
        public async Task ShouldUpdateProjectAsync()
        {
            var project = EntityFactory.Project().Save();
            var projectUpdateDtoRequest = ModelFactory.ProjectUpdateDto()
                .RuleFor(p => p.Id, project.Id)
                .Generate();
        
            var (response, result) = await Request.PutAsync<Result>(new Uri($"{Uri}/{project.Id}"), projectUpdateDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
              
            await DbContext.Entry(project).ReloadAsync();
            CompareFactory.ProjectEqualsProjectUpdateDto().Equals(projectUpdateDtoRequest, project);
        }
        
        [Fact]
        public async Task ShouldUpdateWorkProjectAsync()
        {
            var project = EntityFactory.Project().Save();
            var work = project.DeveloperProjects.Single().Works.Single();
            var workClientDtoRequest = ModelFactory.WorkClientDto().Generate();
        
            var (response, result) = await Request.PutAsync<Result>(new Uri($"{Uri}/{project.Id}/works/{work.Id}"), workClientDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
        }
        
        [Fact]
        public async Task ShouldDeleteAsync()
        {
            var project = EntityFactory.Project().Save();
        
            var (response, result) = await Request.DeleteAsync<Result>(new Uri($"{Uri}/{project.Id}"));
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
            var existProject = await DbContext.Projects.AnyAsync(p => p.Id == project.Id);
            Assert.False(existProject);
        }
        
        [Fact]
        public async Task ShouldDeleteAsync1()
        {
            var project = EntityFactory.Project().Save();
            var work = project.DeveloperProjects.Single().Works.Single();

            var (response, result) = await Request.DeleteAsync<Result>(new Uri($"{Uri}/{project.Id}/works/{work.Id}"));
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
            var existWork = await DbContext.Works.AnyAsync(p => p.Id == work.Id);
            Assert.False(existWork);
        }
    }
}