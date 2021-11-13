using System;
using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Domain._Common.Results;
using Tasks.Domain.Developers.Dtos.Auth;
using System.Threading.Tasks;
using Xunit;

namespace Tasks.API.IntegrationTests.Controllers
{
    public class AuthControllerTests : TestBase
    {
        public AuthControllerTests(TasksAPIFixture fixture) : base(fixture, "api/auth") { }
        
        [Fact]
        public async Task ShouldReturnResultAsync()
        {
            var (response, result) = await Request.GetAsync<Result>(new Uri($"{Uri}/logout"));
        
            response.EnsureSuccessStatusCode();
            Assert.NotEqual(default, result);
        }
        
        [Fact]
        public async Task ShouldLoginAsync()
        {
            var developer = EntityFactory.Developer().Save();
            var loginDtoRequest = ModelFactory.LoginDto()
                .RuleFor(r => r.Login, developer.Login)
                .RuleFor(r => r.Password, "123")
                .Generate();

            var (response, result) = await Request.PostAsync<Result<TokenDto>>(new Uri($"{Uri}/login"), loginDtoRequest);
        
            response.EnsureSuccessStatusCode();
            var modelResponse = result?.Data;
            Assert.NotEqual(default, modelResponse);
        }
    }
}