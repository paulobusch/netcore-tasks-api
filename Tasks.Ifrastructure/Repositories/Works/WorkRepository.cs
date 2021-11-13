using Tasks.Domain.Works.Entities;
using Tasks.Domain.Works.Repositories;
using Tasks.Ifrastructure._Common.Repositories;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.Ifrastructure.Repositories.Works
{
    public class WorkRepository : RepositoryBase<Work>, IWorkRepository
    {
        public WorkRepository(TasksContext context) : base(context) { }
    }
}
