using Microsoft.Extensions.Hosting;
using RedisClone.Core.Extensions;
using RedisClone.Infrastructure;
using RedisClone.Persistance;

var builder = Host.CreateApplicationBuilder();

builder.Services
    .AddConfiguration()
    .AddBaseRedis()
    .AddTcpConnector()
    .AddDatabase()
    .AddCommands();

using var host = builder.Build();
await host.RunAsync();