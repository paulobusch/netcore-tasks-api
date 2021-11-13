using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Developers.Repositories;
using Tasks.Ifrastructure._Common.Repositories;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.Ifrastructure.Repositories.Developers
{
    public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(TasksContext context) : base(context) { }

        public async Task<bool> ExistByLoginAsync(string login, Guid ignoreId = default)
        {
            return await _context.Developers.AnyAsync(d => d.Login == login && d.Id != ignoreId);
        }

        public async Task<Developer> FindByLoginAsync(string login)
        {
            return await _context.Developers.SingleAsync(d => d.Login == login);
        }
    }
}
