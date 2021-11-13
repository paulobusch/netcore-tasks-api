using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Domain._Common.External;
using Tasks.Domain._Common.Security;

namespace Tasks.CrossCutting
{
    public static class ConfigureSettings
    {
        public static void ConfigureTokenJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("Token").Get<TokenConfiguration>());
        }

        public static void ConfigureMocky(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("Mocky").Get<MockyConfiguration>());
        }
    }
}
