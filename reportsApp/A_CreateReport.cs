using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using ServerlessApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reportsApp
{
    public static class A_CreateReport
    {
        [FunctionName("A_CreateReport")]
        public static async Task<Report> CreateReport(
            [ActivityTrigger] List<Payment> payments,
            IBinder binder, ILogger log)
        {
            log.LogInformation($"Executing A_CreateReport");

            var cardType = payments.Select(p => p.CardType).First();
            var reportId = Guid.NewGuid().ToString();
            var reportResourceUri = $"reports/{cardType}/{reportId}.txt";

            using (var report = binder.Bind<TextWriter>(new BlobAttribute(reportResourceUri)))
            {
                report.WriteLine($"Total payments with {cardType}: {payments.Count}");
                report.WriteLine($"Total amount payed: ${payments.Sum(p => p.Amount)}");
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Utils.GetEnvironmentVariable("AzureWebJobsStorage"));

            return new Report
            {
                CardType = cardType,
                Url = $"{storageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri}{reportResourceUri}"
            };
        }
    }
}
