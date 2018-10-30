using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ServerlessApp.Models;

namespace ServerlessApp
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static void Run([BlobTrigger("licences/{name}.lic", Connection = "")]Stream myBlob, string name, 
            [Table("orders", "stripe", "{name}")] Order order,
            TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
