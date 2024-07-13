namespace RedisClone.Core.Abstractions.Sockets.Adapters;

public interface IStreamAdapter : IAsyncDisposable
{
    Task<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken);
    Task WriteAsync(Memory<byte> buffer, CancellationToken cancellationToken);
}