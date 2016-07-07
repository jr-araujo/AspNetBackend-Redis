using BackendRedis.Crosscutting.Caching;
using BackendRedis.Crosscutting.Model;
using Newtonsoft.Json;

namespace BackendWebJobs
{
    public class ProcessQueue
    {
        private readonly RedisConnectionManager RedisManager;

        public ProcessQueue(RedisConnectionManager redisManager)
        {
            RedisManager = redisManager;
        }

        public void PersistPresentation(Presentation presentation)
        {
            using (var redisClient = RedisManager.RedisConnection)
            {
                redisClient.SetEntryInHash("events:" + presentation.EventName + ":presentation",
                    presentation.Id.ToString(), JsonConvert.SerializeObject(presentation));
            }
        }
    }
}
