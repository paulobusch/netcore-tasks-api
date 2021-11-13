using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos;
using System.Threading.Tasks;
using Xunit;

namespace Tasks.API.IntegrationTests.Controllers
{
    public class DevelopersControllerTests : TestBase
    {
        public DevelopersControllerTests(TasksAPIFixture fixture) : base(fixture, "api/developers") { }
        
        [Fact]
        public async Task ShouldReturnManyAsync()
        {
            var developer = EntityFactory.Developer().Save();
            var paginationDtoRequest = ModelFactory.PaginationDto().Generate();
        
            var (response, result) = await Request.GetAsync<Result<IEnumerable<DeveloperListDto>>>(Uri, paginationDtoRequest);
        
            response.EnsureSuccessStatusCode();
            var listResponse = result?.Data;
            Assert.NotEqual(default, listResponse);
            Assert.NotEmpty(listResponse);
            var modelResponse = listResponse.Single(d => d.Id == developer.Id);
            CompareFactory.DeveloperEqualsDeveloperListDto().Equals(developer, modelResponse);
        }
        
        [Fact]
        public async Task ShouldReturnByIdAsync()
        {
            var developer = EntityFactory.Developer().Save();
        
            var (response, result) = await Request.GetAsync<Result<DeveloperDetailDto>>(new Uri($"{Uri}/{developer.Id}"));
        
            response.EnsureSuccessStatusCode();
            var modelResponse = result?.Data;
            Assert.NotEqual(default, modelResponse);
            CompareFactory.DeveloperEqualsDeveloperDetailDto().Equals(developer, modelResponse);
        }
        
        [Fact]
        public async Task ShouldCreateAsync()
        {
            var developerCreateDtoRequest = ModelFactory.DeveloperCreateDto().Generate();
        
            var (response, result) = await Request.PostAsync<Result>(Uri, developerCreateDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
              
            var developer = await DbContext.Developers.FindAsync(developerCreateDtoRequest.Id);
            CompareFactory.DeveloperEqualsDeveloperCreateDto().Equals(developerCreateDtoRequest, developer);
        }
        
        [Fact]
        public async Task ShouldUpdateAsync()
        {
            var developer = EntityFactory.Developer().Save();
            var developerUpdateDtoRequest = ModelFactory.DeveloperUpdateDto()
                .RuleFor(d => d.Id, developer.Id)
                .Generate();
        
            var (response, result) = await Request.PutAsync<Result>(new Uri($"{Uri}/{developer.Id}"), developerUpdateDtoRequest);
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
              
            await DbContext.Entry(developer).ReloadAsync();
            CompareFactory.DeveloperEqualsDeveloperUpdateDto().Equals(developerUpdateDtoRequest, developer);
        }
        
        [Fact]
        public async Task ShouldDeleteAsync()
        {
            var developer = EntityFactory.Developer().Save();
        
            var (response, result) = await Request.DeleteAsync<Result>(new Uri($"{Uri}/{developer.Id}"));
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
            var existDeveloper = await DbContext.Developers.AnyAsync(d => d.Id == developer.Id);
            Assert.False(existDeveloper);
        }
    }
}