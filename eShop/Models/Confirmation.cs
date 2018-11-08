using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Models
{
    public class Confirmation
    {
        public string ChargeId { get; set; }
        public string Email { get; set; }
        public string Product { get; set; }
    }
}
