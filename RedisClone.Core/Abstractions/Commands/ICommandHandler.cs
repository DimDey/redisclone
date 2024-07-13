using System.Collections.ObjectModel;
using RedisClone.Core.Types.Aggregate;

namespace RedisClone.Core.Abstractions.Commands;

public interface ICommandHandler
{
    Memory<byte> HandleCommand(Memory<byte> commandName, ReadOnlyCollection<BulkString> args);
}