using System.Text;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Types.Aggregate;
using RedisClone.Core.Types.Simple;

namespace RedisClone.Core.Types;

public abstract class RedisObject : IRedisObject
{
    public abstract Memory<byte> Serialize();
    public abstract IRedisObject Deserialize(Memory<byte> obj, out int endIndex);

    public static RedisObject Parse(Memory<byte> obj, out int endIndex)
    {
        return obj.Span[0] switch
        {
            (byte)SimpleString.Prefix => new SimpleString().Deserialize(obj, out endIndex),
            (byte)BulkString.Prefix => new BulkString().Deserialize(obj, out endIndex),
            (byte)ArrayObject.Prefix => new ArrayObject().Deserialize(obj, out endIndex),
            _ => throw new ArgumentOutOfRangeException($"Unknown type with prefix {obj.Span[0]}")
        };
    }

    protected int GetObjectSize(Span<byte> obj, int startIndex, out int endSizeIndex)
    {
        var origStartIndex = startIndex;
        while (obj[startIndex] >= '0' && obj[startIndex] <= '9')
        {
            startIndex++;
        }

        var sizeBytes = obj[origStartIndex..startIndex];
        var strBytes = Encoding.ASCII.GetString(sizeBytes);
        endSizeIndex = startIndex + 2;
        return int.Parse(strBytes);
    }
}