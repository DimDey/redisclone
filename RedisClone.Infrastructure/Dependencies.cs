using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Abstractions.Sockets;
using RedisClone.Core.Abstractions.Sockets.Adapters;
using RedisClone.Core.Options;
using RedisClone.Infrastructure.Adapters;
using RedisClone.Infrastructure.Database;
using RedisClone.Infrastructure.Services;

namespace RedisClone.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddBaseRedis(this IServiceCollection services)
    {
        services.AddHostedService<ConnectionListenerService>();
        services.AddScoped<IConnectionService, ConnectionService>();

        services.AddScoped<IMessageHandler, MessageHandler>();
        services.AddSingleton<ICommandHandler, CommandHandler>();

        return services;
    }

    public static IServiceCollection AddTcpConnector(this IServiceCollection services)
    {
        services.AddSingleton<TcpListener>(x =>
        {
            var socketOptions = x.GetRequiredService<IOptions<SocketServerOptions>>().Value;
            return new TcpListener(IPAddress.Any, socketOptions.Port);
        });
        services.AddSingleton<IConnectorAdapter, TcpConnector>();
        

        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IConcurrentDatabase, ConcurrentDatabase>();

        return services;
    }
}