using GabBsb2016.Crosscutting.Caching;
using GabBsb2016.Crosscutting.Model;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;

namespace GabBsb2016WebJobs
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
