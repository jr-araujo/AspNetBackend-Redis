using System;
using Microsoft.AspNet.Mvc;
using GabBsb2016BackEnd.Crosscutting.Caching;
using ServiceStack.Redis;
using System.Collections.Generic;
using Newtonsoft.Json;
using GabBsb2016BackEnd.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GabBsb2016BackEnd.Controllers
{
    public class EventController : Controller
    {
        private readonly RedisConnectionManager RedisManager;

        public EventController(RedisConnectionManager redisManager)
        {
            RedisManager = redisManager;
        }

        [HttpGet("event")]
        public IActionResult GetEvents()
        {
            List<string> eventsInJson = new List<string>();

            try
            {
                using (var redisClient = RedisManager.RedisConnection)
                {
                    var keys = redisClient.GetHashKeys("events");

                    foreach (var key in keys)
                    {
                        eventsInJson.Add(redisClient.GetValueFromHash("events", key));
                    }

                    return Ok(eventsInJson);
                }
            }
            catch (Exception)
            {
                return HttpBadRequest(new
                {
                    Code = 1001,
                    Message = "Sorry :( - An error ocurred when trying to retrieve the events."
                });
            }
        }

        [HttpPost("event")]
        public IActionResult CreateEvent([FromBody]Event @event)
        {
            try
            {
                using (var redisClient = RedisManager.RedisConnection)
                {
                    if (redisClient.HashContainsEntry("events", @event.Name))
                    {
                        return HttpBadRequest(new
                        {
                            Code = 409,
                            Message = "Sorry :( - Already exists another event with same name, Choice other Name !"
                        });
                    }

                    redisClient.SetEntryInHash("events", @event.Name, JsonConvert.SerializeObject(@event));

                    return Ok(@event);
                }
            }
            catch (Exception)
            {
                return HttpBadRequest(new
                {
                    Code = 1001,
                    Message = "Sorry :( - An error ocurred when trying to retrieve the events."
                });
            }
        }
    }
}
