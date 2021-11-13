using Tasks.Domain._Common.Enums;
using Tasks.Domain.Developers.Dtos.Auth;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class LoginDtoFaker : Faker<LoginDto> 
    {
        public LoginDtoFaker()
        {
            RuleFor(l => l.Status, f => f.Random.Enum<Status>())
                // .RuleFor(l => l.Models, () => new DeveloperCreateDtoFaker().Generate(1))
                // .RuleFor(l => l.Update, new DeveloperUpdateDtoFaker())
                .RuleFor(l => l.Login, f => f.Person.UserName)
                .RuleFor(l => l.Password, f => f.Internet.Password());
        } 
    }
}