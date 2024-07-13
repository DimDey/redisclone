using System.Text;
using Microsoft.Extensions.Logging;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Abstractions.Sockets;
using RedisClone.Core.Types;
using RedisClone.Core.Types.Aggregate;

namespace RedisClone.Infrastructure.Services;

public class MessageHandler(
    ICommandHandler commandHandler,
    ILogger<MessageHandler> logger)
    : IMessageHandler
{
    public Memory<byte>? HandleRequest(Memory<byte> data)
    {
        var message = RedisObject.Parse(data, out _);
        if (message is not ArrayObject array)
        {
            logger.LogError("Non array type unsupported!");
            return null;
        }
        
        logger.LogDebug($"Handling message: {Encoding.UTF8.GetString(data.Span)}");

        if (array[0] is { } commandName) 
            return commandHandler.HandleCommand(commandName.Value, array.Args);
        
        logger.LogInformation($"Command name is null: {data.ToArray()}");
        return null;
    }
}