using Tasks.Domain.Works.Dtos;
using Bogus;
using System;

namespace Tasks.API.IntegrationTests.Fakers.Models 
{
    public class WorkClientDtoFaker : Faker<WorkClientDto> 
    {
        public WorkClientDtoFaker()
        {
            RuleFor(w => w.Id, f => f.Random.Guid())
                .RuleFor(w => w.StartTime, new DateTime(2021, 5, 1))
                .RuleFor(w => w.EndTime, new DateTime(2021, 5, 2))
                .RuleFor(w => w.Comment, f => f.Lorem.Word())
                .RuleFor(w => w.Hours, f => f.Random.Float());
        } 
    }
}