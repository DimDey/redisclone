using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedisClone.Core.Abstractions.Sockets;
using RedisClone.Core.Abstractions.Sockets.Adapters;
using RedisClone.Core.Buffers;
using RedisClone.Core.Options;

namespace RedisClone.Infrastructure.Services;

public class ConnectionService(IMessageHandler messageHandler, IOptions<SocketServerOptions> options, ILogger<ConnectionService> logger) : IConnectionService
{
    private readonly SocketServerOptions _options = options.Value;

    public async Task ListenClient(IConnectionAdapter client, CancellationToken cancellationToken)
    {
        await using var clientStream = client.GetStream();
        using var buffer = new NetworkBuffer(_options.ReadBufferLength);
        
        int readChunkLength;
        do
        {
            readChunkLength = await clientStream.ReadAsync(buffer.ReadChunk, cancellationToken);
            var response = messageHandler.HandleRequest(buffer.ReadChunk);
            if (response == null)
                continue;
            
            logger.LogDebug($"Response message: {Encoding.UTF8.GetString(response.Value.Span)}");
            
            cancellationToken.ThrowIfCancellationRequested();
            await clientStream.WriteAsync(response.Value, cancellationToken);
        } while (readChunkLength > 0 && !cancellationToken.IsCancellationRequested);
    }
}