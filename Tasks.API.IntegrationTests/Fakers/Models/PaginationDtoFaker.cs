using Tasks.Domain._Common.Dtos;
using Bogus;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class PaginationDtoFaker : Faker<PaginationDto> 
    {
        public PaginationDtoFaker()
        {
            RuleFor(p => p.Page, 1)
                .RuleFor(p => p.Limit, f => f.Random.Int(1, 100));
        } 
    }
}