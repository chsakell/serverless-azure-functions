﻿using System;

namespace ePaymentsApp.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string CardType { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string StripeCustomerId { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
