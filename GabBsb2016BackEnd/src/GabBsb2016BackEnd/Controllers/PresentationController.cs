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
    public class PresentationController : Controller
    {
        private readonly RedisConnectionManager RedisManager;

        public PresentationController(RedisConnectionManager redisManager)
        {
            RedisManager = redisManager;
        }

        [HttpGet("presentation/{eventKey}")]
        public IActionResult GetPresentations(string eventKey)
        {
            var hashKey = "events:" + eventKey + ":presentation";

            List<Presentation> presentationsInJson = new List<Presentation>();

            try
            {
                using (var redisClient = RedisManager.RedisConnection)
                {
                    var keys = redisClient.GetHashKeys(hashKey);

                    foreach (var key in keys)
                    {
                        presentationsInJson.Add(JsonConvert.DeserializeObject<Presentation>(redisClient.GetValueFromHash(hashKey, key)));
                    }

                    return Ok(presentationsInJson);
                }
            }
            catch (Exception)
            {
                return HttpBadRequest(new
                {
                    Code = 1001,
                    Message = "Sorry :( - An error ocurred when trying to retrieve the presentations."
                });
            }
        }

        [HttpPost("presentation")]
        public IActionResult CreatePresentation([FromBody]Presentation presentation)
        {
            try
            {
                using (var redisClient = RedisManager.RedisConnection)
                {
                    if (redisClient.HashContainsEntry("events:" + presentation.EventName + ":presentation", presentation.Id.ToString()))
                    {
                        return HttpBadRequest(new
                        {
                            Code = 409,
                            Message = "Sorry :( - Already exists other presentation with same Id !"
                        });
                    }

                    redisClient.SetEntryInHash("events:" + presentation.EventName + ":presentation", presentation.Id.ToString(), JsonConvert.SerializeObject(presentation));

                    return Ok(presentation);
                }
            }
            catch (Exception)
            {
                return HttpBadRequest(new
                {
                    Code = 1001,
                    Message = "Sorry :( - An error ocurred when trying to retrieve the presentations."
                });
            }
        }
    }
}
