namespace RedisClone.Core.Abstractions.Sockets.Adapters;

public interface IConnectorAdapter
{
    void Start();
    void Stop();
    
    Task<IConnectionAdapter> AcceptConnection(CancellationToken cancellationToken);
}