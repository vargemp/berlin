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
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            Deliverer[] DelivererRoutes = FindRandomRoutes(20);
            List<int> RouteCosts = DelivererRoutes.Select(r => r.GetRouteCost()).ToList();

            //List<int> RouteCosts = new List<int>();

            int[] TournamentRoutes = TournamentSelection(3, RouteCosts);
            List<Deliverer> RouletteRoutes = RouletteSelection(DelivererRoutes);


            Console.ReadKey();
        }

        static List<Deliverer> RouletteSelection(Deliverer[] DelivererRoutes)
        {
            //znajduje najgorszego
            int LongestRouteCost = DelivererRoutes.Max(d => d.GetRouteCost()) + 1;
            //int sum = 0;

            List<DelivererRate> List = new List<DelivererRate>();

            for (int i = 0; i < DelivererRoutes.Count(); i++)
            {
                DelivererRate delivererRate = new DelivererRate(DelivererRoutes[i], LongestRouteCost - DelivererRoutes[i].GetRouteCost());
                List.Add(delivererRate);
            }

            int SumOfInversedCost = List.Sum(s => s.InversedCost);

            List<Deliverer> List2 = new List<Deliverer>();
            
            for (int i = 0; i < List.Count(); i++)
            {
                int j = 0;
                int random = rnd.Next(SumOfInversedCost);
                int sumRandom = List[j].InversedCost;
                while (sumRandom < random)
                {
                    j++;
                    sumRandom += List[j].InversedCost;
                }
                List2.Add(List[j].Deliverer);                
            }


            //List<Deliverer> FoundElements = new List<Deliverer>();
            ////losuje 
            //for (int i = 0; i < DelivererRoutes.Length; i++)
            //{
            //    int randomNum = rnd.Next(LongestRouteCost);
            //    int range = 0;
            //    int j = 0;
            //    bool found = false;

            //    while (!found)
            //    {
            //        range += List[j].InversedCost;
            //        if (randomNum < range)
            //        {
            //            FoundElements.Add(List[j].Deliverer);
            //            found = true;
            //        }
            //        j++;
            //    }
            //}
            return List2;
        }

        static int[] TournamentSelection(int k, List<int> RouteCosts)
        {
            int[] TournamentRoutes = new int[RouteCosts.Count];

            Random rnd = new Random();
            for (int i = 0; i < TournamentRoutes.Length; i++)
            {
                int[] RandomKRoutes = new int[k];
                for (int j = 0; j < k; j++)
                {
                    int randomDeliverer = rnd.Next(RouteCosts.Count);
                    RandomKRoutes[j] = RouteCosts[randomDeliverer];
                }
                TournamentRoutes[i] = RandomKRoutes.Min();
            }
            return TournamentRoutes;
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
        private static string[][] getDistanceArray()
        {
            string fileName = "berlin52.txt";

            var lines = File.ReadAllLines(fileName);
            string[][] array = new string[lines.Length - 1][];
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
