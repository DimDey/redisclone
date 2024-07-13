using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Types.Aggregate;
using RedisClone.Core.Types.Simple;

namespace RedisClone.Core.Commands;

public class PingCommand : ICommand
{
    public string Name => "ping";
    public IRedisObject Handle(IReadOnlyList<BulkString> args)
        => SimpleString.FromString("PONG");
}