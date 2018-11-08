using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eShop.Models;
using Stripe;

namespace eShop.Controllers
{
    public class HomeController : Controller
    {
        static List<string> RandomNames = new List<string>
        {
            "Abe Poorman", "Errol Medeiros", "Kraig Stultz", "Corazon Alligood", "Buster Turco",
            "Velma Cunniff", "Tona Nealon", "Eveline Gaydos", "Kurtis Hornbaker", "Teressa Suitt"
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, 
                                                long amountInCents, string productName)
        {
            Random r = new Random();
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var dbCustomerId = r.Next(0, 10);

            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = await chargeService.CreateAsync(new ChargeCreateOptions
            {
                Amount = amountInCents,
                Description = "Azure Functions Payment",
                Currency = "usd",
                CustomerId = customer.Id,
                Metadata = new Dictionary<string, string> {
                    { "id", dbCustomerId.ToString() },
                    { "name", RandomNames[dbCustomerId] },
                    { "product", productName }
                }
            });

            var confirmation = new Confirmation()
            {
                ChargeId = charge.Id,
                Email = customer.Email,
                Product = productName
            };

            return View(confirmation);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
