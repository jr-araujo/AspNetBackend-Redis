using GabBsb2016.Crosscutting.Configuration;
using Microsoft.Extensions.OptionsModel;
using ServiceStack.Redis;

namespace GabBsb2016.Crosscutting.Caching
{
    public class RedisConnectionManager
    {
        private readonly object InternalLock = new object();
        private IRedisClientsManager Connection { get; set; }
        private readonly IOptions<Config> Options;

        public RedisConnectionManager(IOptions<Config> options)
        {
            Options = options;
        }

        public IRedisClient RedisConnection
        {
            get
            {
                if (Connection == null)
                {
                    lock (InternalLock)
                    {
                        if (Connection == null)
                        {
                            var connectionString = $"{Options.Value.Redis.Password}@{Options.Value.Redis.Host}:{Options.Value.Redis.Port}";

                            Connection = new PooledRedisClientManager(new string[] { connectionString });
                        }
                    }
                }

                return Connection.GetClient();
            }
        }
    }
}