using Microsoft.Extensions.Logging;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Database;
using RedisClone.Core.Extensions;
using RedisClone.Core.Types.Aggregate;
using RedisClone.Core.Types.Simple;

namespace RedisClone.Core.Commands;

public class SetCommand(
    IConcurrentDatabase concurrentDatabase,
    ILogger<SetCommand> logger)
    : ICommand
{
    private const string ExParameter = "ex";
    
    public string Name => "set";

    public IRedisObject Handle(IReadOnlyList<BulkString> args)
    {
        if (args.Count < 2)
            throw new ArgumentNullException("Key and value is null");

        try
        {
            var key = args[0].Value;
            var data = args[1].Value;
            var expireTime = args.TryGetExpireTime(ExParameter);
            
            concurrentDatabase.TrySet(key, data, expireTime);
        }
        catch (Exception ex)
        {
            logger.LogError($"Catched exception while set command parse: {ex.Message} | {ex.StackTrace}");
            return SimpleString.Bad;
        }

        return SimpleString.Ok;
    }

    
}