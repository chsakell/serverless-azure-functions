using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessApp.Models
{
    public class Order : Transaction
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
