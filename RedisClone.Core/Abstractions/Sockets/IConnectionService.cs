using RedisClone.Core.Abstractions.Sockets.Adapters;

namespace RedisClone.Core.Abstractions.Sockets;

public interface IConnectionService
{
    Task ListenClient(IConnectionAdapter client, CancellationToken cancellationToken);
}