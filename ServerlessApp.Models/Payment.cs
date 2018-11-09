using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessApp.Models
{
    public class Payment: TableEntity
    {
        public string ChargeId { get; set; }
        public string CardType { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Product { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
