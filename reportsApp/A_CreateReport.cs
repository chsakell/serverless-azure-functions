using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ServerlessApp.Models;
using System;
using System.Collections.Generic;
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
            TraceWriter log)
        {
            await Task.Delay(5000);

            var cardType = payments.Select(p => p.CardType).First();
            var format = "pdf";

            switch (cardType)
            {
                case "Visa":
                    format = "xls";
                    break;
                case "Mastercard":
                    format = "docx";
                    break;
                default:
                    break;
            }

            return new Report
            {
                CardType = cardType,
                Format = format,
                Url = $"{cardType}_Payments.{format}"
            };
        }
    }
}
