using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Dtos;
using Tasks.Domain._Common.Enums;
using Tasks.Domain.Developers.Dtos;
using Tasks.Domain.Developers.Dtos.Ranking;
using Tasks.Domain.Developers.Dtos.Works;
using Tasks.IntegrationTests._Common;
using Tasks.IntegrationTests._Common.Results;
using Tasks.UnitTests._Common.Random;
using Xunit;

namespace Tasks.IntegrationTests.Developers
{
public class DevelopersControllerTests : BaseTest
{
    public DevelopersControllerTests(TasksFixture fixture) : base(fixture, "developers") { }

    [Fact]
    public async void GetByIdAsync()
    {
        var developer = EntitiesFactory.NewDeveloper().Save();

        var (status, result) = await Request.GetAsync<ResultTest<DeveloperDetailDto>>(
            new Uri($"{Uri}/{developer.Id}")
        );
            
        var developerResult = result.Data;
        Assert.Equal(Status.Success, status);
        Assert.Equal(developer.Id, developerResult.Id);
        Assert.Equal(developer.Name, developerResult.Name);
        Assert.Equal(developer.Login, developerResult.Login);
        Assert.Equal(developer.CPF, developerResult.CPF);
    }

    [Fact]
    public async void ListAsync()
    {
        EntitiesFactory.NewDeveloper().Save();
        EntitiesFactory.NewDeveloper().Save();
        var query = new PaginationDto { Page = 1, Limit = 1 };
        var expectedTotal = await DbContext.Developers.CountAsync();

        var (status, result) = await Request.GetAsync<ResultTest<IEnumerable<DeveloperListDto>>>(Uri, query);

        var developerList = result.Data;
        Assert.Equal(Status.Success, status);
        Assert.NotEmpty(developerList);
        Assert.Equal(expectedTotal, result.TotalRows);
        Assert.True(developerList.Count() == query.Limit);
    }

    [Fact]
    public async void CreateAsync()
    {
        var developerDto = new DeveloperCreateDto { 
            Id = Guid.NewGuid(),
            Login = RandomHelper.RandomString(),
            Name = RandomHelper.RandomString(),
            Password = RandomHelper.RandomNumbers(),
            CPF = RandomHelper.RandomNumbers(11)
        };

        var (status, result) = await Request.PostAsync<ResultTest>(Uri, developerDto);

        var developerDb = await DbContext.Developers.FindAsync(developerDto.Id);
        Assert.Equal(Status.Success, status);
        Assert.True(result.Success);
        Assert.Equal(developerDto.Login, developerDb.Login);
        Assert.Equal(developerDto.Name, developerDb.Name);
        Assert.Equal(developerDto.CPF, developerDb.CPF);
    }

    [Fact]
    public async void UpdateAsync()
    {
        var developer = EntitiesFactory.NewDeveloper().Save();
        var developerDto = new DeveloperUpdateDto
        {
            Id = developer.Id,
            Login = RandomHelper.RandomString(),
            Name = RandomHelper.RandomString()
        };

        var (status, result) = await Request.PutAsync<ResultTest>(new Uri($"{Uri}/{developer.Id}"), developerDto);

        var developerDb = await DbContext.Developers.FindAsync(developerDto.Id);
        await DbContext.Entry(developerDb).ReloadAsync();
        Assert.Equal(Status.Success, status);
        Assert.True(result.Success);
        Assert.Equal(developerDto.Login, developerDb.Login);
        Assert.Equal(developerDto.Name, developerDb.Name);
    }

    [Fact]
    public async void DeleteAsync()
    {
        var developer = EntitiesFactory.NewDeveloper().Save();

        var (status, result) = await Request.DeleteAsync<ResultTest>(new Uri($"{Uri}/{developer.Id}"));

        var existDeveloper = await DbContext.Developers.AnyAsync(d => d.Id == developer.Id);
        Assert.Equal(Status.Success, status);
        Assert.True(result.Success);
        Assert.False(existDeveloper);
    }
}
}
