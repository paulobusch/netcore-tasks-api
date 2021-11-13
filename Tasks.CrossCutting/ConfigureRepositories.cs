using Microsoft.Extensions.DependencyInjection;
using Tasks.Domain._Common.Interfaces;
using Tasks.Domain.Developers.Repositories;
using Tasks.Domain.Projects.Repositories;
using Tasks.Domain.Works.Repositories;
using Tasks.Ifrastructure._Common.Repositories;
using Tasks.Ifrastructure.Repositories.Developers;
using Tasks.Ifrastructure.Repositories.Projects;
using Tasks.Ifrastructure.Repositories.Works;

namespace Tasks.CrossCutting
{
    public static class ConfigureRepositories
    {

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IWorkRepository, WorkRepository>();
        }
    }
}
