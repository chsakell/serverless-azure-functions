using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace reportsApp
{
    public static class S_GenerateReports
    {
        [FunctionName("S_GenerateReports")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var orders = await GetOrders();

            var orchestrationId = await starter.StartNewAsync("O_GenerateReports", null);

            return starter.CreateCheckStatusResponse(req, orchestrationId);
        }

        private static async Task<List<Transaction>> GetOrders()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("orders");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<Transaction> query = new TableQuery<Transaction>();

            List<Transaction> results = new List<Transaction>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<Transaction> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        
    }


    public class Transaction : TableEntity
    {
        public string Id { get; set; }
        public string CardType { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string StripeCustomerId { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
