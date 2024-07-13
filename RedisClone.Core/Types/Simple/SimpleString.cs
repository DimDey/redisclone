using System.Text;

namespace RedisClone.Core.Types.Simple;

public class SimpleString : RedisObject
{
    public static SimpleString Ok = FromString("OK");
    public static SimpleString Bad = FromString("BAD");
    
    public const char Prefix = '+';
    private Memory<byte> Value { get; set; }
    
    public override Memory<byte> Serialize()
    {
        var totalByteCount = 1 + Value.Span.Length + 2;
        var result = new byte[totalByteCount];
        result[0] = (byte)Prefix;
        Value.Span.CopyTo(result.AsSpan(1));
        result[totalByteCount - 2] = (byte)'\r';
        result[totalByteCount - 1] = (byte)'\n';

        return result;
    }

    public override SimpleString Deserialize(Memory<byte> obj, out int endIndex)
    {
        var span = obj.Span;
        var index = 1;
        while (span[index] != '\r') {
            index++;
        }
        Value = obj[1..index];
        endIndex = index + 2;
        return this;
    }

    public static SimpleString FromMemory(Memory<byte> data)
    {
        return new SimpleString
        {
            Value = data
        };
    }
    
    public static SimpleString FromString(string data)
    {
        return new SimpleString
        {
            Value = Encoding.UTF8.GetBytes(data).AsMemory()
        };
    }
}