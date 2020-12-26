using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis; 

namespace AuthLibrary.Services
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

                ConfigurationOptions configuration = ConfigurationOptions.Parse(configString);
                configuration.Password = "AwesomePassw0rd123";
                _redis = ConnectionMultiplexer.Connect(configuration);
            }
            catch (RedisConnectionException err)
            {
                logger.LogError(err.ToString());
                throw err;
            }

            logger.LogDebug("Connected to Redis");
        }

        public async Task<bool> Set(string key, TimeSpan expiry, string tokenJti, List<dynamic> resources)
        {
            var db = _redis.GetDatabase();
            var redisValue = await db.StringGetAsync(key);
            IList<string> jtiList = new List<string>();
            if (redisValue.IsNullOrEmpty)
            {
                var jti = tokenJti;
                jtiList.Add(jti);
            }
            else
            {
                var dbValue = (dynamic) JsonConvert.DeserializeObject(redisValue.ToString());
                var array = ((dbValue as dynamic).jtis as dynamic) as JArray;
                array.Add(tokenJti);
                jtiList = array.ToObject<List<string>>();
            }

            var value = new {jtis = jtiList, resources = resources};
            var stringSet = await db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
            return stringSet;
        }

        public async Task<bool> RemoveJti(string key, string jti)
        {
            var db = _redis.GetDatabase();
            var redisValue = await db.StringGetAsync(key);
            var dbValue = (dynamic) JsonConvert.DeserializeObject(redisValue.ToString());
            var array = ((dbValue as dynamic).jtis as dynamic) as JArray;
            var jtiList = array.ToObject<List<string>>();
            jtiList.Remove(jti);
            var value = new {jtis = jtiList, resources = ((dbValue as dynamic).resources as JValue).ToString()};
            var stringSet = await db.StringSetAsync(key, JsonConvert.SerializeObject(value));
            return stringSet;
        }

        public async Task<string> Get(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<List<object>> Search(string key)
        {
            var keys = await SearchRedisKeys(key);
            var db = _redis.GetDatabase();
            List<RedisValue> redisValues = db.StringGet(keys.ToArray()).ToList();
            List<object> values = new List<object>();
            foreach (var value in redisValues)
            {
                var s = value.ToString();
                //var o = JsonConvert.DeserializeObject<List<ApplicationPermissionViewModel>>(s);

                values.Add(s);
            }

            return values;
        }

        public async Task<List<RedisKey>> SearchRedisKeys(string key)
        {
            var server = _redis.GetServer(_redisHost, _redisPort);
            IAsyncEnumerator<RedisKey> keysAsync =
                server.KeysAsync(pattern: $"{key}*", pageSize: 100).GetAsyncEnumerator();
            List<RedisKey> keys = new List<RedisKey> { };
            while (await keysAsync.MoveNextAsync())
            {
                keys.Add(keysAsync.Current);
            }

            return keys;
        }
    }
}