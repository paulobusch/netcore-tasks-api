using Tasks.Ifrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using Bogus;

namespace Tasks.API.IntegrationTests.Common.Abstract
{
    public class EntityFakerBase<TEntity> : Faker<TEntity> where TEntity : class
    {
        private readonly TasksContext _context;

        public EntityFakerBase(TasksContext context)
        {
            _context = context;

            CustomInstantiator(f => Activator.CreateInstance(typeof(TEntity), nonPublic: true) as TEntity);
        }

        public TEntity Save()
        {
            var entity = Generate();

            _context.Add(entity);
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
            return entity;
        }
    }
}