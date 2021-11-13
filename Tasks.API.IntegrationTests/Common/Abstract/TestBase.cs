using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tasks.API.IntegrationTests.Assertions;
using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.API.IntegrationTests.Fakers;
using Tasks.Domain.Developers.Services;
using Tasks.Ifrastructure.Contexts;
using Xunit;

namespace Tasks.API.IntegrationTests.Common.Abstract
{
    public class TestBase : IClassFixture<TasksAPIFixture>, IAsyncLifetime
    {
        public readonly Uri Uri;
        public readonly Request Request;
        public readonly TasksContext DbContext;

        public readonly ModelFakerFactory ModelFactory;
        public readonly EntityFakerFactory EntityFactory;
        public readonly CompareFactory CompareFactory;
        public readonly IServiceProvider Services;
        public static Guid DeveloperId;

        public TestBase(TasksAPIFixture fixture, string url)
        {
            Request = fixture.Request;
            DbContext = fixture.DbContext;
            Services = fixture.Services;
            Uri = new Uri($"{fixture.Client.BaseAddress}{url}");

            ModelFactory = new ModelFakerFactory();
            EntityFactory = new EntityFakerFactory(this);
            CompareFactory = new CompareFactory();
        }

        private static string _token;
        protected virtual void Login()
        {
            var developer = EntityFactory.Developer().Save();
            var authService = Services.GetRequiredService<IAuthService>();
            _token = authService.GenerateJwtTokenAsync(developer).Result.Token;
            DeveloperId = developer.Id;

            Request.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        protected virtual void Logout()
        {
            Request.Client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task InitializeAsync()
        {
            await DbContext.Database.EnsureCreatedAsync();
            Login();
        }

        public async Task DisposeAsync()
        {
            await DbContext.Database.EnsureDeletedAsync();
        }
    }
}