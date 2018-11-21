using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ServerlessApp.Models;

namespace ePaymentsApp
{
    public static class ProcessSuccessCharge
    {
        [FunctionName("ProcessSuccessCharge")]
        public static void Run([QueueTrigger("success-charges", Connection = "")]
        Transaction transaction, IBinder binder, 
        [Table("payments")] out Payment payment,
        ILogger log)
        {
            log.LogInformation($"ProcessSuccessCharge function processed: {transaction}");

            payment = new Payment
            {
                PartitionKey = "stripe",
                RowKey = transaction.Id,
                ChargeId = transaction.ChargeId,
                Amount = transaction.Amount,
                CardType = transaction.CardType,
                Currency = transaction.Currency,
                CustomerEmail = transaction.CustomerEmail,
                CustomerId = transaction.CustomerId,
                CustomerName = transaction.CustomerName,
                Product = transaction.Product,
                DateCreated = transaction.DateCreated
            };

            using (var license = binder.Bind<TextWriter>(new BlobAttribute($"licenses/{transaction.Id}.lic")))
            {
                license.WriteLine($"Transaction ID: {transaction.ChargeId}");
                license.WriteLine($"Email: {transaction.CustomerEmail}");
                license.WriteLine($"Amount payed: {transaction.Amount}  {transaction.Currency}");
                license.WriteLine($"license key: {transaction.Id}");
            }
        }
    }
}
