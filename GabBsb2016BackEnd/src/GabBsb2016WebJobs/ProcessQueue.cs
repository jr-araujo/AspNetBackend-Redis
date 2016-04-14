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
            //try
            //{
                using (var redisClient = RedisManager.RedisConnection)
                {
                    if (redisClient.HashContainsEntry("events:" + presentation.EventName + ":presentation",
                        presentation.Id.ToString()))
                    {
                        //return;
                        //return HttpBadRequest(new
                        //{
                        //    Code = 409,
                        //    Message = "Sorry :( - Already exists other presentation with same Id !"
                        //});
                    }

                    redisClient.SetEntryInHash("events:" + presentation.EventName + ":presentation",
                        presentation.Id.ToString(), JsonConvert.SerializeObject(presentation));

                    //return Ok(presentation);
                }
            //}
            //catch
            //{
            //    return HttpBadRequest(new
            //    {
            //        Code = 1001,
            //        Message = "Sorry :( - An error ocurred when trying to retrieve the presentations."
            //    });
            //}
        }
    }
}
