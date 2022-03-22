using System.Net;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Talaryon.Data;

namespace Remato
{
    public enum RematoConfigCache
    {
        Memory,
        Redis
    }

    public enum RematoConfigDatabase
    {
        Sqlite,
        Mysql,
        Postgres
    }
    
    public class RematoConfig
    {
        public RematoConfigCache Cache { get; set; } = RematoConfigCache.Memory;
        public RematoConfigDatabase Database { get; set; } = RematoConfigDatabase.Sqlite;

        public SqliteOptions SqliteOptions { get; set; } = new() { DataSource = "data.db" };
        public MysqlOptions MysqlOptions { get; set; }
        public RedisCacheOptions RedisOptions { get; set; }
        public TokenOptions TokenOptions { get; set; }

        public IPEndPoint EndPoint { get; set; } = new(IPAddress.Any, 5399);
    }
}