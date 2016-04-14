using GabBsb2016.Crosscutting.Caching;
using GabBsb2016.Crosscutting.Configuration;
using GabBsb2016.Crosscutting.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace GabBsb2016WebJobs
{
    public static class PresentationsQueueProcessor
    {
        public static ProcessQueue ProcessQueue;

        public static void ProcessPresentation(
            [QueueTrigger("presentations")] Presentation presentationEnqueued)
        {
            ProcessQueue.PersistPresentation(presentationEnqueued);
        }
    }
}
