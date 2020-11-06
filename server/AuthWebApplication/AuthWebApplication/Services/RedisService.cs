using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AuthWebApplication.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly int _redisPort;
        private ConnectionMultiplexer _redis;
        private ILogger<RedisService> logger;

        public RedisService(IConfiguration config, ILogger<RedisService> logger)
        {
            _redisHost = config["Redis:Host"];
            _redisPort = Convert.ToInt32(config["Redis:Port"]);
            this.logger = logger;
        }

        public void Connect()
        {
            try
            {
                var configString = $"{_redisHost}:{_redisPort},connectRetry=5";
                _redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (RedisConnectionException err)
            {
                logger.LogError(err.ToString());
                throw err;
            }

            logger.LogDebug("Connected to Redis");
        }

        public async Task<bool> Set(string key, string value)
        {
            var db = _redis.GetDatabase();
            var stringSet = await db.StringSetAsync(key, value);
            return stringSet;
        }

        public async Task<string> Get(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }
    }
}
