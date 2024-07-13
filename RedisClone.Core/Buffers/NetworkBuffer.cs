using System.Buffers;

namespace RedisClone.Core.Buffers;

public sealed class NetworkBuffer(int readBufferLength) : IDisposable
{
    private readonly byte[] _readChunk = ArrayPool<byte>.Shared.Rent(readBufferLength);
    
    public Memory<byte> ReadChunk => _readChunk.AsMemory();

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_readChunk);
        GC.SuppressFinalize(this);
    }
}