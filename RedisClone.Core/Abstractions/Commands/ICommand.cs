using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Types.Aggregate;

namespace RedisClone.Core.Abstractions.Commands;

public interface ICommand
{
    public string Name { get; }

    public IRedisObject Handle(IReadOnlyList<BulkString> args);
}