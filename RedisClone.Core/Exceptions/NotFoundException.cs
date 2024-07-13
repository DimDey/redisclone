namespace RedisClone.Core.Exceptions;

public class NotFoundException(string key) : Exception($"Not found {key}")
{
    public string Key { get; } = key;
}