using Tasks.Domain.Projects.Dtos.Works;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class ProjectWorkSearchDtoFaker : Faker<ProjectWorkSearchDto> 
    {
        public ProjectWorkSearchDtoFaker()
        {
            RuleFor(p => p.Page, 1)
                .RuleFor(p => p.Limit, f => f.Random.Int(1, 100));
        } 
    }
}