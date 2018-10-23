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
            string[][] distanceArray = getDistanceArray();
            for (int i = 0; i < DeliverersNumber; i++)
            {
                DelivererRoutes[i] = new Deliverer(distanceArray);
            }

            return DelivererRoutes;
        }

        private static string[][] getDistanceArray()
        {
            string fileName = "berlin52.txt";

            var lines = File.ReadAllLines(fileName);
            string[][] array = new string[lines.Length][];
            string[][] array2 = new string[lines.Length][];
            for (var i = 1; i < lines.Length; i += 1)
            {
                var line = lines[i];
                array[i - 1] = line.Split(' ');
                Array.Resize(ref array[i - 1], array[i - 1].Length - 1);
            }

            return array;
        }
    }
}
