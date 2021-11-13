using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Tasks.API;
using Tasks.CrossCutting;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Developers.Services;
using Tasks.Ifrastructure._Common.Application;
using Tasks.Ifrastructure.Contexts;
using Tasks.IntegrationTests._Common.Tools;
using Tasks.UnitTests._Common.Factories;

namespace Tasks.IntegrationTests
{
    public class TasksFixture : IDisposable
    {
        public Request Request { get; private set; }
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public TasksContext DbContext { get; private set; }
        public EntitiesFactory EntitiesFactory { get; private set; }
        public Developer SessionDeveloper { get; private set; }
        
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;

        public TasksFixture()
        {
            _configuration = Configuration.GetConfiguration("Test");

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(Project.GetDirectory("Tasks.API"))
                .ConfigureServices(ConfigureServices)
                .UseConfiguration(_configuration)
                .UseEnvironment("Test")
                .UseStartup(typeof(Startup));

            Server = new TestServer(webHostBuilder);
            _services = Server.Services;
            Client = Server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:44325/api");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken());
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Request = new Request(Client);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(typeof(Startup).GetTypeInfo().Assembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider(),
                    new ViewComponentFeatureProvider()
                }
            };

            services.AddDatabases(_configuration);

            var scope = services
                .BuildServiceProvider()
                .CreateScope();

            var scopedServices = scope.ServiceProvider;
            DbContext = scopedServices.GetRequiredService<TasksContext>();
            EntitiesFactory = new EntitiesFactory(DbContext);

            services.AddSingleton(manager);
        }

        private string GenerateToken()
        {
            SessionDeveloper = EntitiesFactory.NewDeveloper().Save();
            var authService = _services.GetService<IAuthService>();
            return authService.GenerateJwtTokenAsync(SessionDeveloper).Result.Token;
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}
