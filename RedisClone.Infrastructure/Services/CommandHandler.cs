using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Extensions.Logging;
using RedisClone.Core.Abstractions.Commands;
using RedisClone.Core.Types.Aggregate;
using RedisClone.Core.Types.Simple;

namespace RedisClone.Infrastructure.Services;

public class CommandHandler(
    IEnumerable<ICommand> commands,
    ILogger<CommandHandler> logger)
    : ICommandHandler
{
    public Memory<byte> HandleCommand(Memory<byte> commandName, ReadOnlyCollection<BulkString> args)
    {
        var commandHandler = commands.FirstOrDefault(x => 
            Encoding.UTF8.GetString(commandName.Span).Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        if (commandHandler == null) return SimpleString.Bad.Serialize();
        try
        {
            var responseMessage = commandHandler.Handle(args);
            return responseMessage.Serialize();
        }
        catch (Exception ex)
        {
            logger.LogError("ERR: {Message} | {StackTrace} | {Data} for {Command} {Args}", 
                ex.Message, 
                ex.StackTrace, ex.Data,
                commandName,
                args.Select(x => x.Value).ToArray());
            return SimpleString.Bad.Serialize();
        }
    }
}