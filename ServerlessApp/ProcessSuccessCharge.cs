using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessApp
{
    public static class ProcessSuccessCharge
    {
        [FunctionName("ProcessSuccessCharge")]
        public static void Run([QueueTrigger("success-charges", Connection = "")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
