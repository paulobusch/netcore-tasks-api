using System;
using System.Threading.Tasks;
using Tasks.Domain._Common.Interfaces;
using Tasks.Domain.Projects.Entities;

namespace Tasks.Domain.Projects.Repositories
{
    public interface IProjectRepository : IRepository<Project> {
        Task<bool> ExistByTitleAsync(string title, Guid ignoreId = default);
        Task<bool> ExistDeveloperVinculatedAsync(Guid id, Guid developerId);
        Task<Guid> GetDeveloperProjectIdAsync(Guid id, Guid developerId);
    }
}
