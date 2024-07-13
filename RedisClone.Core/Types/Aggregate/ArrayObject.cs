using System.Collections.ObjectModel;

namespace RedisClone.Core.Types.Aggregate;

public class ArrayObject : RedisObject
{
    public const char Prefix = '*';
    private readonly List<RedisObject> _args = [];
    
    public ReadOnlyCollection<BulkString?> Args => _args
        .Skip(1)
        .Select(x => x as BulkString)
        .ToList()
        .AsReadOnly();
    
    public BulkString this[int index] => _args[index] as BulkString ?? new BulkString();
    
    public override Memory<byte> Serialize() => throw new NotImplementedException();

    public override RedisObject Deserialize(Memory<byte> obj, out int endIndex)
    {
        var numOfElements = GetObjectSize(obj.Span, 1, out var endSizeIndex);
        endIndex = endSizeIndex;
        for (var i = 0; i < numOfElements; i++)
        {
            var element = Parse(obj[endIndex..], out var elementEndIndex);
            _args.Insert(i, element);
            endIndex += elementEndIndex;
        }
        
        return this;
    }
}