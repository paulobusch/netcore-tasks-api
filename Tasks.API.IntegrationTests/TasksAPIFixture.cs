using Xunit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using Tasks.API.IntegrationTests.Common.Utils;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.API.IntegrationTests
{
    public class TasksAPIFixture : ICollectionFixture<TasksAPIFixture>, IAsyncLifetime
    {
        public Request Request { get; private set; }
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public TasksContext DbContext { get; private set; }

        public readonly IConfiguration Configuration;
        public readonly IServiceProvider Services;

        public TasksAPIFixture()
        {
            var projectName = "Tasks.API";
            Configuration = ConfigurationLoader.GetConfiguration(projectName, "Test");

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(ProjectExplorer.GetDirectory(projectName))
                .ConfigureTestServices(ConfigureTestServices)
                .UseConfiguration(Configuration)
                .UseEnvironment("Test")
                .UseStartup(typeof(Startup));

            Server = new TestServer(webHostBuilder);
            Services = Server.Services;
            DbContext = Services.GetRequiredService<TasksContext>();
            Client = Server.CreateClient();
            Client.BaseAddress = new Uri($"https://localhost:8000");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Request = new Request(Client);
        }

        public async Task InitializeAsync() { 
            await Task.CompletedTask; 
        }

        private void ConfigureTestServices(IServiceCollection services) { }

        public async Task DisposeAsync()
        {
            await DbContext.DisposeAsync();
            Client.Dispose();
            Server.Dispose();
        }
    }
}