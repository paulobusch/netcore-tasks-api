using Tasks.Domain.Projects.Dtos;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class ProjectUpdateDtoFaker : Faker<ProjectUpdateDto> 
    {
        public ProjectUpdateDtoFaker()
        {
            RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph());
        } 
    }
}