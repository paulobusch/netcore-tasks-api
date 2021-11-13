using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Ifrastructure.Contexts;
using Tasks.API.IntegrationTests.Fakers.Entities;

namespace Tasks.API.IntegrationTests.Fakers
{
    public class EntityFakerFactory
    {
        private readonly TasksContext _context;

        public EntityFakerFactory(TestBase test)
        {
            _context = test.DbContext;
        }
        public DeveloperFaker Developer() => new(_context);
        public ProjectFaker Project() => new(_context);
    }
}