using System.IO;
using System.Text;
using System.Collections.Generic;

namespace xCarpaccio.client
{
    using Nancy;
    using System;
    using Nancy.ModelBinding;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => "It works !!! You need to register your server on main server.";

            Post["/order"] = _ =>
            {
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    Console.WriteLine("Order received: {0}", reader.ReadToEnd());
                }

                var order = this.Bind<Order>();
                Bill bill = null;
                //TODO: do something with order and return a bill if possible
                // If you manage to get the result, return a Bill object (JSON serialization is done automagically)
                // Else return a HTTP 404 error : return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                
                decimal tot = 0;

                for (int i = 0; i < order.Prices.Length; i++)
                {
                    var price = order.Prices[i] * order.Quantities[i];
                    tot += price;
                }

                decimal taxes = 0;
                switch (order.Country)
                {
                    case "DE":
                    case "FR":
                    case "RO":
                    case "NL":
                    case "EL":
                    case "LV":
                    case "MT":
                        taxes = 1.2m;
                        break;
                    case "UK":
                    case "PL":
                    case "BG":
                    case "DK":
                    case "IE":
                    case "CY":
                        taxes = 1.21m;
                        break;
                }

                if (order.Country == "DE" || 
                    order.Country == "FR" || 
                    order.Country == "RO" || 
                    order.Country == "NL" || 
                    order.Country == "EL" || 
                    order.Country == "LV" || 
                    order.Country == "MT")
                {

                    tot = tot * 1.2m;

                    if (order.Reduction == "STANDARD")
                    {
                        if (tot >= 1000 && tot < 5000)
                        {
                            tot = tot * 0.97m;
                        }
                        if (tot >= 5000 && tot < 7000)
                        {
                            tot = tot * 0.95m;
                        }
                        if (tot >= 7000 && tot < 10000)
                        {
                            tot = tot * 0.93m;
                        }
                        if (tot >= 10000 && tot < 50000)
                        {
                            tot = tot * 0.90m;
                        }
                        if (tot >= 50000)
                        {
                            tot = tot * 0.85m;
                        }
                    }
                    Console.WriteLine(tot);
                    bill = new Bill();
                    bill.total = tot;

                }
                
                return bill;
            };

            Post["/feedback"] = _ =>
            {
                var feedback = this.Bind<Feedback>();
                Console.Write("Type: {0}: ", feedback.Type);
                Console.WriteLine(feedback.Content);
                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}