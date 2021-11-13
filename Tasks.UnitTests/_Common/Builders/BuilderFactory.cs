using Microsoft.EntityFrameworkCore;
using Tasks.Domain._Common.Entities;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.UnitTests._Common.Builders
{
    public class BuilderFactory<TEntity> where TEntity : EntityBase
    {
        private readonly TasksContext _context;
        private readonly TEntity _entity;

        public BuilderFactory(
            TEntity entity,
            TasksContext context
        )
        {
            _context = context;
            _entity = entity;
        }

        public TEntity Get() => _entity;

        public TEntity Save()
        {
            _context.Add(_entity);
            _context.Entry(_entity).State = EntityState.Added;
            _context.SaveChanges();
            return _entity;
        }
    }
}
