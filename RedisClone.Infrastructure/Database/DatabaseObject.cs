namespace RedisClone.Infrastructure.Database;

internal sealed record DatabaseObject
{
    public Memory<byte> Data { get; init; }
    
    public DateTime? ExpireOn { get; init; }
}