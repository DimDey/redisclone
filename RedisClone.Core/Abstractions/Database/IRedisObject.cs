namespace RedisClone.Core.Abstractions.Database;

public interface IRedisObject
{
    public Memory<byte> Serialize();
    public IRedisObject Deserialize(Memory<byte> obj, out int endIndex);
}