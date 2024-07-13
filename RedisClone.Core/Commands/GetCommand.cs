using Microsoft.Extensions.Logging;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Exceptions;
using RedisClone.Core.Types.Aggregate;

namespace RedisClone.Core.Commands;

public class GetCommand(
    IConcurrentDatabase concurrentDatabase,
    ILogger<GetCommand> logger)
    : ICommand
{
    public string Name => "get";

    public IRedisObject Handle(IReadOnlyList<BulkString> args)
    {
        if (args.Count <= 0)
            throw new ArgumentNullException("Key is null");

        try
        {
            var data = concurrentDatabase.TryGet(args[0].Value);
            return BulkString.FromMemory(data);
        }
        catch (NotFoundException ex)
        {
            logger.LogError($"Not found element with key {ex.Key}");
            return BulkString.Null;
        }
    }
}