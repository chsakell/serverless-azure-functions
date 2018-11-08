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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, 
                                                long amountInCents, string productName)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = await chargeService.CreateAsync(new ChargeCreateOptions
            {
                Amount = amountInCents,
                Description = "ASP.NET Core Stripe",
                Currency = "usd",
                CustomerId = customer.Id,
                Metadata = new Dictionary<string, string> {
                    { "id", "1" },
                    { "name", "Christos" },
                    { "product", productName }
                }
            });

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
