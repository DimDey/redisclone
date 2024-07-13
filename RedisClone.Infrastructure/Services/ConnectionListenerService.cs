using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedisClone.Core.Abstractions.Sockets;
using RedisClone.Core.Abstractions.Sockets.Adapters;
using RedisClone.Core.Options;

namespace RedisClone.Infrastructure.Services;

public class ConnectionListenerService : BackgroundService
{
    private readonly ILogger<ConnectionListenerService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConnectorAdapter _connector;
    
    public ConnectionListenerService(
        IOptions<SocketServerOptions> socketOptions, 
        ILogger<ConnectionListenerService> logger, 
        IServiceScopeFactory serviceScopeFactory, 
        IConnectorAdapter connector)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _connector = connector;
        _logger.LogInformation($"Server started at port {socketOptions.Value.Port}");
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start listening clients..");
        
        await ListenConnections(cancellationToken);
    }
    
    private async Task ListenConnections(CancellationToken cancellationToken)
    {
        _connector.Start();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var serviceScope = _serviceScopeFactory.CreateScope();
                using var client = await _connector.AcceptConnection(cancellationToken);
                
                var connectionService = serviceScope.ServiceProvider.GetRequiredService<IConnectionService>();
                _ = connectionService.ListenClient(client, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} | StackTrace: {ex.StackTrace}");
            }
        }

        _connector.Stop();
    }
}