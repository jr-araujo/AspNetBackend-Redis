using GabBsb2016.Crosscutting.Model;
using Microsoft.Azure.WebJobs;

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
