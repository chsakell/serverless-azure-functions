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
    public static class O_GenerateReports
    {
        [FunctionName("O_GenerateReports")]
        public static async Task<List<Report>> GenerateReports(
           [OrchestrationTrigger] DurableOrchestrationContext ctx,
           TraceWriter log)
        {
            var payments = ctx.GetInput<List<Payment>>();
            var reportTasks = new List<Task<Report>>();

            foreach (var paymentGroup in payments.GroupBy(p => p.CardType))
            {
                var task = ctx.CallActivityAsync<Report>("A_CreateReport", paymentGroup.ToList());
                reportTasks.Add(task);
            }

            var reports = await Task.WhenAll(reportTasks);

            return reports.ToList();
        }
    }
}
