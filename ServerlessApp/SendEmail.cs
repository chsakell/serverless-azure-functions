using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using SendGrid.Helpers.Mail;
using ServerlessApp.Models;

namespace ePaymentsApp
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static void Run([BlobTrigger("licences/{name}.lic")]CloudBlockBlob licenceBlob, 
            string name, 
            [Table("payments", "stripe", "{name}")] Payment payment,
            [SendGrid] out SendGridMessage message,
            ILogger log)
        {
            log.LogInformation($"SendEmail Blob trigger function processing blob: {name}");
            message = new SendGridMessage();
            message.AddTo(System.Environment.GetEnvironmentVariable("EmailRecipient", EnvironmentVariableTarget.Process));
            message.AddContent("text/html", $"Download your licence <a href='{licenceBlob.Uri.AbsoluteUri}' alt='Licence link'>here</a>");
            message.SetFrom(new EmailAddress("payments@chsakell.com"));
            message.SetSubject("Your payment has been completed");
        }
    }
}
