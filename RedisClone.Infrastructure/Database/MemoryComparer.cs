namespace RedisClone.Infrastructure.Database;

internal class MemoryComparer : IEqualityComparer<Memory<byte>>
{
    private const int HashCodePrefix = 17;
    private const int HashCodeOffset = 31;
    
    public bool Equals(Memory<byte> x, Memory<byte> y)
    {
        return x.Span.SequenceEqual(y.Span);
    }

    public int GetHashCode(Memory<byte> obj)
    {
        var span = obj.Span;
        unchecked
        {
            var hash = HashCodePrefix;
            foreach (var b in span)
                hash = hash * HashCodeOffset + b;
            
            return hash;
        }
    }
}