namespace ePaymentsApp.Models
{
    public class Order : Transaction
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
