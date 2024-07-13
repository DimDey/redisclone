namespace RedisClone.Core.Abstractions.Sockets;

public interface IMessageHandler
{
    Memory<byte>? HandleRequest(Memory<byte> data);
}