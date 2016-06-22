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

                Dictionary<string, decimal> taxesdict = new Dictionary<string, decimal>();

                taxesdict.Add("FI", 1.17m);
                taxesdict.Add("SK", 1.18m);
                taxesdict.Add("ES", 1.19m);
                taxesdict.Add("CZ", 1.19m);
                taxesdict.Add("DE", 1.20m);
                taxesdict.Add("FR", 1.20m);
                taxesdict.Add("RO", 1.20m);
                taxesdict.Add("NL", 1.20m);
                taxesdict.Add("EL", 1.20m);
                taxesdict.Add("LV", 1.20m);
                taxesdict.Add("MT", 1.20m);
                taxesdict.Add("UK", 1.21m);
                taxesdict.Add("PL", 1.21m);
                taxesdict.Add("BG", 1.21m);
                taxesdict.Add("DK", 1.21m);
                taxesdict.Add("IE", 1.21m);
                taxesdict.Add("CY", 1.21m);
                taxesdict.Add("AT", 1.22m);
                taxesdict.Add("EE", 1.22m);
                taxesdict.Add("LT", 1.23m);
                taxesdict.Add("HR", 1.23m);
                taxesdict.Add("SE", 1.23m);
                taxesdict.Add("PT", 1.23m);
                taxesdict.Add("BE", 1.24m);
                taxesdict.Add("SI", 1.24m);
                taxesdict.Add("LU", 1.25m);
                taxesdict.Add("IT", 1.25m);
                taxesdict.Add("HU", 1.27m);

                if (taxesdict.ContainsKey(order.Country))
                {
                    decimal taxes = taxesdict[order.Country];

                    tot = tot * taxes;

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