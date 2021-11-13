using System;
using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Domain.Developers.Entities;
using Tasks.Domain.Projects.Entities;
using Tasks.Domain.Works.Entities;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.API.IntegrationTests.Fakers.Entities 
{
    public class ProjectFaker : EntityFakerBase<Project> 
    {
        public ProjectFaker(TasksContext context) : base(context)
        {
            var projectId = Guid.NewGuid();
            var work = new EntityFakerBase<Work>(context)
                .RuleFor(w => w.StartTime, new DateTime(2021, 5, 1))
                .RuleFor(w => w.EndTime, new DateTime(2021, 5, 2))
                .RuleFor(w => w.Comment, f => f.Lorem.Word())
                .RuleFor(w => w.Hours, f => f.Random.Float())
                .Generate();

            var developerProject = new EntityFakerBase<DeveloperProject>(context)
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.DeveloperId, TestBase.DeveloperId)
                .RuleFor(d => d.ProjectId, projectId)
                .RuleFor(d => d.Works, new [] { work })
                .Generate();
            
            CustomInstantiator(f => new Project(default, default, default, new [] { developerProject }));

            RuleFor(p => p.Id, projectId)
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Word());
        } 
    }
}