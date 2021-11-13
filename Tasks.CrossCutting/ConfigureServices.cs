using Microsoft.Extensions.DependencyInjection;
using Tasks.Domain._Common.Session;
using Tasks.Domain.Developers.Services;
using Tasks.Domain.External.Services;
using Tasks.Domain.Projects.Services;
using Tasks.Domain.Works.Services;
using Tasks.Service.Developers;
using Tasks.Service.External;
using Tasks.Service.Projects;
using Tasks.Services._Common.Session;
using Tasks.Services.Works;

namespace Tasks.CrossCutting
{
    public static class ConfigureServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAutenticationContext, AutenticationContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMockyService, MockyService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IWorkService, WorkService>();
        }
    }
}
