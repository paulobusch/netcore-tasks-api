using System;
using System.IdentityModel.Tokens.Jwt;
using Tasks.Domain._Common.Enums;
using Tasks.Domain.Developers.Dtos.Auth;
using Tasks.IntegrationTests._Common;
using Tasks.IntegrationTests._Common.Results;
using Tasks.IntegrationTests._Common.Tools;
using Tasks.UnitTests._Common.Random;
using Xunit;

namespace Tasks.IntegrationTests.Developers
{
    public class AuthControllerTests : BaseTest
    {
        public AuthControllerTests(TasksFixture fixture) : base(fixture, "auth/login") { }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void LoginDeveloper(bool equalPassword) {
            var loginDto = new LoginDto { 
                Login = RandomHelper.RandomString(),
                Password = RandomHelper.RandomNumbers()
            };
            var developer = EntitiesFactory.NewDeveloper(
                login: loginDto.Login,
                password: equalPassword ? loginDto.Password : null
            ).Save();

            var (status, result) = await Request.PostAsync<ResultTest<TokenDto>>(Uri, loginDto);
            Assert.Equal(equalPassword ? Status.Success : Status.Unauthorized, status);
            if (equalPassword)
            {
                var loginResult = result.Data;
                Assert.Equal(developer.Id, loginResult.Id);
                Assert.Equal(developer.Login, loginResult.Login);
                Assert.Equal(TimeSpan.FromMinutes(5), loginResult.ExpiresAt - loginResult.CreatedAt);
                Assert.NotEmpty(loginResult.Token);
                var jwt = new JwtSecurityToken(loginResult.Token);
                Assert.NotEmpty(jwt.Claims);
            }
        }
    }
}
