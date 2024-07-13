using System.Net.Sockets;
using RedisClone.Core.Abstractions.Sockets.Adapters;

namespace RedisClone.Infrastructure.Adapters;

public class TcpClientAdapter(TcpClient client) : IConnectionAdapter
{
    public IStreamAdapter GetStream() => new NetworkStreamAdapter(client.GetStream());

    public void Dispose() => client.Dispose();
    
}