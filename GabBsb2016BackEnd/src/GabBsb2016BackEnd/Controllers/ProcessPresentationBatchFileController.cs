using GabBsb2016.Crosscutting.Configuration;
using GabBsb2016.Crosscutting.Model;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.IO;

namespace GabBsb2016BackEnd.Controllers
{
    public class ProcessPresentationBatchFileController : Controller
    {
        private readonly IOptions<Config> Options = null;
        public ProcessPresentationBatchFileController(IOptions<Config> options)
        {
            Options = options;
        }

        [HttpPost("send-presentation-batch")]
        public IActionResult EnqueuePresentation(IFormFile file)
        {
            using (var presentationBatch = new StreamReader(file.OpenReadStream()))
            {
                string[] lineValues;
                string line = "";
                while ((line = presentationBatch.ReadLine()) != null)
                {
                    lineValues = line.Split(';');

                    EnqueuePresentation(lineValues[0], lineValues[1], lineValues[2], lineValues[3],
                        lineValues[4]);
                }
            }

            return Ok(new
            {
                ProcessingStatusCode = 1000,
                Messagem = "File receipt successfully and the content was enqueued."
            });
        }

        private void EnqueuePresentation(string title, string description, string speakerName,
            string room, string eventName)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(Options.Value.StorageEndpoint.Endpoint);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("presentations");
            queue.CreateIfNotExists();

            var presentation = new Presentation
            {
                Title = title,
                Description = description,
                SpeakerName = speakerName,
                Room = room,
                EventName = eventName
            };

            CloudQueueMessage msg = new CloudQueueMessage(JsonConvert.SerializeObject(presentation)
              );

            queue.AddMessage(msg);
        }
    }
}