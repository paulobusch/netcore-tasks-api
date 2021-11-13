using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Projects.Entities;
using Tasks.Domain.Works.Entities;
using Tasks.Ifrastructure.Seeders;

namespace Tasks.Ifrastructure.Contexts
{
    public class TasksContext : DbContext
    {
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Work> Works { get; set; }

        public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            new TasksSeed().Seed(builder);

            base.OnModelCreating(builder);
        }
    }
}
