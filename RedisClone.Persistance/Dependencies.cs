using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisClone.Core.Options;

namespace RedisClone.Persistance;

public static class Dependencies
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();
            
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<SocketServerOptions>()
            .Bind(configuration.GetSection(SocketServerOptions.SectionName));

        return services;
    }
}