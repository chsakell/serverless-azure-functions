using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Stripe;
using System.Threading.Tasks;
using System.Net.Http;
using ePaymentsApp.Models;

namespace ePaymentsApp
{
    public static class OnSuccessCharge
    {
        [FunctionName("OnSuccessCharge")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req,
            [Queue("success-charges", Connection = "AzureWebJobsStorage")]IAsyncCollector<Transaction> queue,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var jsonEvent = await req.Content.ReadAsStringAsync();

            var @event = EventUtility.ParseEvent(jsonEvent);

            var charge = @event.Data.Object as Charge;
            var card = charge.Source as Card;

            var transaction = new Transaction
            {
                Id = charge.Id,
                Amount = charge.Amount,
                Currency = charge.Currency,
                DateCreated = charge.Created,
                StripeCustomerId = charge.CustomerId,
                CustomerEmail = card.Name,
                CardType = card.Brand,
                CustomerId = int.Parse(charge.Metadata["id"]),
                CustomerName = charge.Metadata["name"]
            };

            await queue.AddAsync(transaction);
        }
    }
}
