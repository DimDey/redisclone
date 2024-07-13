using System.Text;
using RedisClone.Core.Types.Aggregate;

namespace RedisClone.Core.Extensions;

public static class BulkStringExtensions
{
    public static int TryGetExpireTime(this IReadOnlyList<BulkString> args, string exParameter)
    {
        if (args.Count < 4)
            return -1;

        var parameter = Encoding.UTF8.GetString(args[2].Value.Span);
        if (parameter != exParameter) return -1;
        
        var expireTimeSpan = args[3].Value.Span;
        if (TryParseIntFromSpan(expireTimeSpan, out var expireTime))
            return expireTime * 1000;

        return -1;
    }
    
    private static bool TryParseIntFromSpan(ReadOnlySpan<byte> span, out int value)
    {
        value = 0;
        var sign = 1;
        var i = 0;

        if (span.Length == 0)
            return false;

        if (span[0] == '-')
        {
            sign = -1;
            i = 1;
        }

        for (; i < span.Length; i++)
        {
            if (span[i] < '0' || span[i] > '9')
                return false;

            value = value * 10 + (span[i] - '0');
        }

        value *= sign;
        return true;
    }
}