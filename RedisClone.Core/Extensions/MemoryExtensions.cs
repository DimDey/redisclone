using System.Collections.Immutable;

namespace RedisClone.Core.Extensions;

public static class MemoryExtensions
{
    public static Memory<byte> Join(this IImmutableList<Memory<byte>> values, char separator)
    {
        if (values.Count == 0)
            return Memory<byte>.Empty;
        var totalLength = values.Sum(x => x.Length) + (values.Count - 1);
        Memory<byte> resultMemory = new byte[totalLength];
        var resultSpan = resultMemory.Span;
        
        var position = 0;
        for (var i = 0; i < values.Count; i++)
        {
            var valueSpan = values[i].Span;
            
            valueSpan.CopyTo(resultSpan[position..]);
            position += valueSpan.Length;

            if (i >= values.Count - 1) continue;
            resultSpan[position] = (byte)separator;
            position++;
        }

        return resultMemory;
    }
}