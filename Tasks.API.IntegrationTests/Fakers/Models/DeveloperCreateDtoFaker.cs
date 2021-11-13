using Tasks.Domain.Developers.Dtos;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class DeveloperCreateDtoFaker : Faker<DeveloperCreateDto> 
    {
        public DeveloperCreateDtoFaker()
        {
            RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Name, f => f.Person.FullName)
                .RuleFor(d => d.Login, f => f.Person.UserName)
                .RuleFor(d => d.CPF, f => f.Lorem.Word())
                .RuleFor(d => d.Password, f => f.Internet.Password());
        } 
    }
}