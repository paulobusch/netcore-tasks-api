using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Tasks.Ifrastructure._Common.Application;

namespace Tasks.Ifrastructure.Contexts
{
    public class ContextFactory : IDesignTimeDbContextFactory<TasksContext>
    {
        private readonly IConfiguration _configuration;

        public ContextFactory()
        {
            _configuration = Configuration.GetConfiguration();
        }

        public TasksContext CreateDbContext(string[] args)
        {
            var connectionString = _configuration.GetConnectionString("Tasks");
            var builder = new DbContextOptionsBuilder<TasksContext>();
            builder.UseMySql(connectionString);
            return new TasksContext(builder.Options);
        }
    }
}
