namespace RedisClone.Core.Abstractions.Database;

public interface IConcurrentDatabase
{
    Memory<byte> TryGet(Memory<byte> memoryKey);

    void TrySet(Memory<byte> memoryKey, Memory<byte> data, int expireTime = -1);
}