using Tasks.Domain.Developers.Dtos;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class DeveloperUpdateDtoFaker : Faker<DeveloperUpdateDto> 
    {
        public DeveloperUpdateDtoFaker()
        {
            RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Name, f => f.Person.FullName)
                .RuleFor(d => d.Login, f => f.Person.UserName);
        } 
    }
}