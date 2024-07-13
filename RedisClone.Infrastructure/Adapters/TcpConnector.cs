using System.Net.Sockets;
using RedisClone.Core.Abstractions.Sockets.Adapters;

namespace RedisClone.Infrastructure.Adapters;

public class TcpConnector(TcpListener listener) : IConnectorAdapter
{
    public void Start() => listener.Start();
    public void Stop() => listener.Stop();
    
    public async Task<IConnectionAdapter> AcceptConnection(CancellationToken cancellationToken)
    {
        return new TcpClientAdapter(await listener.AcceptTcpClientAsync(cancellationToken));
    }
}