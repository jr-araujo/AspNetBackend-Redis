using BackendRedis.Crosscutting.Model;
using Microsoft.Azure.WebJobs;

namespace BackendWebJobs
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
