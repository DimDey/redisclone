using System.Collections.Concurrent;
using System.Text;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Exceptions;

namespace RedisClone.Infrastructure.Database;

public class ConcurrentDatabase : IConcurrentDatabase
{
    private readonly ConcurrentDictionary<Memory<byte>, DatabaseObject> _db = new(new MemoryComparer());
    
    public Memory<byte> TryGet(Memory<byte> memoryKey)
    {
        if (!_db.TryGetValue(memoryKey, out var databaseObject))
            throw new NotFoundException(Encoding.UTF8.GetString(memoryKey.Span));

        if (DateTime.UtcNow > databaseObject.ExpireOn)
            throw new NotFoundException(Encoding.UTF8.GetString(memoryKey.Span));
        
        return databaseObject.Data;
    }

    public void TrySet(Memory<byte> memoryKey, Memory<byte> data, int expireTime = -1)
    {
        var databaseObject = new DatabaseObject
        {
            Data = data.ToArray(),
            ExpireOn = expireTime > 0 ? DateTime.UtcNow + TimeSpan.FromMilliseconds(expireTime) : DateTime.MaxValue
        };

        _db.AddOrUpdate(memoryKey, databaseObject, (k, v) => databaseObject);
    }
}

