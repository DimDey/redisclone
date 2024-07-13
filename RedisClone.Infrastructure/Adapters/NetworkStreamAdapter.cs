using RedisClone.Core.Abstractions.Sockets.Adapters;

namespace RedisClone.Infrastructure.Adapters;

public class NetworkStreamAdapter(Stream stream) : IStreamAdapter
{
    public async Task<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    {
        return await stream.ReadAsync(buffer, cancellationToken);
    }

    public async Task WriteAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    {
        await stream.WriteAsync(buffer, cancellationToken);
    }

    public void Dispose()
    {
        stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await stream.DisposeAsync();
    }
}