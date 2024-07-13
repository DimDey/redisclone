namespace RedisClone.Core.Options;

public class SocketServerOptions
{
    public static string SectionName = "SocketServer";
    
    public int Port { get; set; }

    public int ReadBufferLength { get; set; }

    public string IpAddress { get; set; }
}