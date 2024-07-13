namespace RedisClone.Core.Types.Aggregate;

public class BulkString : RedisObject
{
    public const char Prefix = '$';

    public static readonly BulkString Null = new() { Length = -1 };
    private int Length { get; set; }
    public Memory<byte> Value { get; private set; }
 
    public override Memory<byte> Serialize()
    {
        if (Length == -1)
        {
            // Return the serialized representation of a null BulkString
            return new Memory<byte>("$-1\r\n"u8.ToArray());
        }
        
        var lengthBytes = System.Text.Encoding.UTF8.GetBytes($"{Prefix}{Length}\r\n");
        var dataBytes = Value.ToArray();
        var endingBytes = "\r\n"u8.ToArray();

        var serializedLength = lengthBytes.Length + dataBytes.Length + endingBytes.Length;
        var result = new byte[serializedLength];

        Buffer.BlockCopy(lengthBytes, 0, result, 0, lengthBytes.Length);
        Buffer.BlockCopy(dataBytes, 0, result, lengthBytes.Length, dataBytes.Length);
        Buffer.BlockCopy(endingBytes, 0, result, lengthBytes.Length + dataBytes.Length, endingBytes.Length);

        return new Memory<byte>(result);
    }

    public override RedisObject Deserialize(Memory<byte> obj, out int endIndex)
    {
        Length = GetObjectSize(obj.Span, 1, out var endSizeIndex);
        Value = obj[endSizeIndex..(endSizeIndex + Length)];
        endIndex = endSizeIndex + Length + 2;
        return this;
    }

    public static BulkString FromMemory(Memory<byte> value)
    {
        return new BulkString
        {
            Value = value,
            Length = value.Length
        };
    }
}