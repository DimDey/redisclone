using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RedisClone.Core.Abstractions.Commands;

namespace RedisClone.Core.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services) =>
        services.AddCommands(Assembly.GetExecutingAssembly());
    
    public static IServiceCollection AddCommands(this IServiceCollection services, Assembly assembly)
    {
        var commandType = typeof(ICommand);
        var commands = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && commandType.IsAssignableFrom(t));
        
        foreach (var command in commands)
            services.AddScoped(commandType, command);

        return services;
    }
}