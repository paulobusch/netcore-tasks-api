using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Tasks.Domain._Common.Enums;
using Tasks.Domain._Common.Security;
using Tasks.Domain.Developers.Dtos.Auth;
using Tasks.Domain.Developers.Repositories;
using Tasks.Service.Developers;
using Tasks.UnitTests._Common;
using Tasks.UnitTests._Common.Random;
using Xunit;

namespace Tasks.UnitTests.Developers.Services
{
    public class AuthServiceTests : BaseTest
    {
        private readonly Mock<IDeveloperRepository> _developerRepository;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthServiceTests(TasksFixture fixture) : base(fixture) {
            _developerRepository = new Mock<IDeveloperRepository>();
            _tokenConfiguration = new TokenConfiguration { 
                Issuer = "Issuer",
                Signature = "uY76$657gD7D0YrsF%d8g7",
                Seconds = 60
            };
        }

        public static IEnumerable<object[]> LoginDeveloperData()
        {
            yield return new object[] { Status.Unauthorized };
            yield return new object[] { Status.Unauthorized, true };
            yield return new object[] { Status.Success, true, true };
        }

        [Theory]
        [MemberData(nameof(LoginDeveloperData))]
        public async void LoginDeveloperTest(
            Status expectedStatus,
            bool persistedDeveloper = false,
            bool equalPassword = false
        ) {
            var loginDto = new LoginDto { 
                Login = RandomHelper.RandomString(), 
                Password = RandomHelper.RandomNumbers() 
            };
            var developer = EntitiesFactory.NewDeveloper(
                login: loginDto.Login,
                password: equalPassword ? loginDto.Password : null
            ).Get();
            if (persistedDeveloper)
            {
                _developerRepository.Setup(d => d.ExistByLoginAsync(developer.Login, default)).ReturnsAsync(true);
                _developerRepository.Setup(d => d.FindByLoginAsync(developer.Login)).ReturnsAsync(developer);
            }
            
            var service = new AuthService(_developerRepository.Object, _tokenConfiguration);
            var result = await service.LoginAsync(loginDto);

            var data = result.Data;
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == Status.Success) { 
                Assert.Equal(developer.Id, data.Id);
                Assert.Equal(developer.Login, data.Login);
                Assert.Equal(TimeSpan.FromSeconds(_tokenConfiguration.Seconds), data.ExpiresAt - data.CreatedAt);
                Assert.NotEmpty(data.Token);
                var jwt = new JwtSecurityToken(data.Token);
                Assert.NotEmpty(jwt.Claims);
            } else
            {
                Assert.Null(data?.Token);
            }
        }
    }
}
