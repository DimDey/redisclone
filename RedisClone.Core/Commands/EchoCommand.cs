using System.Collections.Immutable;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Types.Aggregate;
using RedisClone.Core.Types.Simple;
using RedisClone.Core.Extensions;

namespace RedisClone.Core.Commands;

public class EchoCommand : ICommand
{
    public string Name => "echo";
    public IRedisObject Handle(IReadOnlyList<BulkString> args)
    {
        var argStrings = args.Select(x => x.Value).ToImmutableList();
        var result = argStrings.Join(' ');
        return SimpleString.FromMemory(result);
    }
}