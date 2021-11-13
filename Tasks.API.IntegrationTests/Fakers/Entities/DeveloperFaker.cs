using Tasks.API.IntegrationTests.Common.Abstract;
using Tasks.Domain._Common.Crypto;
using Tasks.Domain.Developers.Entities;
using Tasks.Ifrastructure.Contexts;

namespace Tasks.API.IntegrationTests.Fakers.Entities 
{
    public class DeveloperFaker : EntityFakerBase<Developer> 
    {
        public DeveloperFaker(TasksContext context) : base(context)
        {
            RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Name, f => f.Person.FullName)
                .RuleFor(d => d.Login, f => f.Person.UserName)
                .RuleFor(d => d.CPF, f => f.Random.ReplaceNumbers("###########"))
                .RuleFor(d => d.PasswordHash, MD5Crypto.Encode("123"));
            // .RuleFor(d => d.DeveloperProjects, () => new DeveloperProjectFaker().Generate(1))
        } 
    }
}