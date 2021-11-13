using Microsoft.Extensions.Configuration;
using System;

namespace Tasks.Ifrastructure._Common.Application
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration(string environment = default)
        {
            if (string.IsNullOrWhiteSpace(environment))
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return new ConfigurationBuilder()
                .SetBasePath(Project.GetDirectory("Tasks.API"))
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();
        }
    }
}
