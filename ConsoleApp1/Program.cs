using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Deliverer[] DelivererRoutes = FindRandomRoutes(20);
            List<int> RouteCosts = new List<int>();

            foreach (var item in DelivererRoutes)
            {
                foreach (var item2 in item.GetCityList())
                {
                    Console.Write(item2.ToString() + "-");
                }
                Console.WriteLine("Route cost: " + item.GetRouteCost());
                RouteCosts.Add(item.GetRouteCost());
            }
            Console.Write("Average route cost: " + RouteCosts.Average());

            Console.ReadKey();
        }
        
        private static Deliverer[] FindRandomRoutes(int DeliverersNumber)
        {
            Deliverer[] DelivererRoutes = new Deliverer[DeliverersNumber];
            for (int i = 0; i < DeliverersNumber; i++)
            {
                DelivererRoutes[i] = new Deliverer();
            }

            return DelivererRoutes;
        }
    }
}
