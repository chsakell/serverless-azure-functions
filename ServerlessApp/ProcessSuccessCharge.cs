using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ServerlessApp.Models;

namespace ServerlessApp
{
    public static class ProcessSuccessCharge
    {
        [FunctionName("ProcessSuccessCharge")]
        public static void Run([QueueTrigger("success-charges", Connection = "")]
        Transaction transaction,
        IBinder binder,
        [Table("orders")] out Order order,
        TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {transaction}");

            order = new Order
            {
                PartitionKey = "stripe",
                RowKey = transaction.Id,
                Amount = transaction.Amount,
                CardType = transaction.CardType,
                Currency = transaction.Currency,
                CustomerEmail = transaction.CustomerEmail,
                CustomerId = transaction.CustomerId,
                CustomerName = transaction.CustomerName,
                Id = transaction.Id,
                DateCreated = transaction.DateCreated,
                StripeCustomerId = transaction.StripeCustomerId
            };

            using (var licence = binder.Bind<TextWriter>(new BlobAttribute($"licences/{transaction.Id}.lic")))
            {
                licence.WriteLine($"Transaction ID: {transaction.Id}");
                licence.WriteLine($"Email: {transaction.CustomerEmail}");
                licence.WriteLine($"Amount payed: {transaction.Amount}  {transaction.Currency}");
                licence.WriteLine($"Licence key: {Guid.NewGuid().ToString()}");
            }
        }
    }
}
