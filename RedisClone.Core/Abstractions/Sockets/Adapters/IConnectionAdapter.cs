namespace RedisClone.Core.Abstractions.Sockets.Adapters;

public interface IConnectionAdapter : IDisposable
{
    IStreamAdapter GetStream();
}