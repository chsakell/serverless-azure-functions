using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessApp.Models;

namespace reportsApp
{
    public static class S_CreateReports
    {
        [FunctionName("S_CreateReports")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            TraceWriter log)
        {
            log.Info($"S_CreateReports HTTP trigger function executed at: {DateTime.Now}");

            var orders = await GetOrders();

            var orchestrationId = await starter.StartNewAsync("O_GenerateReports", orders);

            return starter.CreateCheckStatusResponse(req, orchestrationId);
        }

        private static async Task<List<Payment>> GetOrders()
        {
            List<Payment> results = new List<Payment>();

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Utils.GetEnvironmentVariable("AzureWebJobsStorage"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("payments");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<Payment> query = new TableQuery<Payment>();
            
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<Payment> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }
    }
}
