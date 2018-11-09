using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ServerlessApp.Models;

namespace ePaymentsApp
{
    public static class ProcessSuccessCharge
    {
        [FunctionName("ProcessSuccessCharge")]
        public static void Run([QueueTrigger("success-charges", Connection = "")]
        Transaction transaction, IBinder binder, 
        [Table("payments")] out Payment payment,
        TraceWriter log)
        {
            log.Info($"ProcessSuccessCharge function processed: {transaction}");

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

            using (var licence = binder.Bind<TextWriter>(new BlobAttribute($"licences/{transaction.Id}.lic")))
            {
                licence.WriteLine($"Transaction ID: {transaction.ChargeId}");
                licence.WriteLine($"Email: {transaction.CustomerEmail}");
                licence.WriteLine($"Amount payed: {transaction.Amount}  {transaction.Currency}");
                licence.WriteLine($"Licence key: {transaction.Id}");
            }
        }
    }
}
