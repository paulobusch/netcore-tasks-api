using System.Collections.Generic;
using Tasks.Domain._Common.Entities;

namespace Tasks.Ifrastructure._Common.Interfaces
{
    public interface ISeeder<TEntity> where TEntity : EntityBase
    {
        public IEnumerable<TEntity> GetList();
    }
}
