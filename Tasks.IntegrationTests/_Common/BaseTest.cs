using System;
using Tasks.Domain.Developers.Entities;
using Tasks.Ifrastructure.Contexts;
using Tasks.IntegrationTests._Common.Tools;
using Tasks.UnitTests._Common.Factories;
using Xunit;

namespace Tasks.IntegrationTests._Common
{
    public class BaseTest : IClassFixture<TasksFixture>
    {
        protected readonly Uri Uri;
        protected readonly Request Request;
        protected readonly EntitiesFactory EntitiesFactory;
        protected readonly TasksContext DbContext;
        protected readonly Developer SessionDeveloper;

        public BaseTest(TasksFixture fixture, string url)
        {
            Request = fixture.Request;
            EntitiesFactory = fixture.EntitiesFactory;
            DbContext = fixture.DbContext;
            SessionDeveloper = fixture.SessionDeveloper;
            Uri = new Uri($"{fixture.Client.BaseAddress}/{url}");
        }
    }
}
