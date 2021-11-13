using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.Domain._Common.Entities;
using Tasks.Domain._Common.Interfaces;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.Ifrastructure._Common.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly TasksContext _context;
        protected readonly DbSet<TEntity> _dataset;

        public RepositoryBase(TasksContext context)
        {
            _context = context;
            _dataset = context.Set<TEntity>();
        }

        public async virtual Task CreateAsync(TEntity entity)
        {
            await _dataset.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(TEntity entity)
        {
            _dataset.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task<bool> ExistAsync(Guid id)
        {
            return await _dataset.AnyAsync(e => e.Id.Equals(id));
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dataset.ToArrayAsync();
        }

        public async virtual Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dataset.FindAsync(id);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return _dataset.AsQueryable();
        }

        public async virtual Task UpdateAsync(TEntity entity)
        {
            var entityDb = await _dataset.FindAsync(entity.Id);
            _context.Entry(entityDb).CurrentValues.SetValues(entity);
            _context.Entry(entityDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
