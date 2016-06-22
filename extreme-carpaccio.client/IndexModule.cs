using System.IO;
using System.Text;

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

                Console.WriteLine(order.Country);
                if (order.Country == "DE"){
                    decimal tot = 0;

                    for (int i = 0; i < order.Prices.Length; i++){
                        var price = order.Prices[i] * order.Quantities[i];
                        price = price * 120 / 100;
                        tot += price;
                    }

                    if (order.Reduction == "STANDARD")
                    {
                        if (tot >= 1000 && tot < 5000)
                        {
                            tot = tot * 97 / 100;
                        }
                        if (tot >= 5000 && tot < 7000)
                        {
                            tot = tot * 95 / 100;
                        }
                        if (tot >= 7000 && tot < 10000)
                        {
                            tot = tot * 93 / 100;
                        }
                        if (tot >= 10000 && tot < 50000)
                        {
                            tot = tot * 90 / 100;
                        }
                        if (tot >= 50000)
                        {
                            tot = tot * 85 / 100;
                        }
                    }

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