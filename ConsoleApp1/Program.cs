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
            List<int> RouteCosts = DelivererRoutes.Select(r => r.GetRouteCost()).ToList();
            //List<int> RouteCosts = new List<int>();

            int[] TournamentRoutes = TournamentSelection(3, RouteCosts);
            int[] RouletteRoutes = RouletteSelection(RouteCosts);

            
            Console.ReadKey();
        }

        static int[] RouletteSelection(List<int> RouteCosts)
        {
            //Calculate S = the sum of a finesses.
            int[] sortedList = RouteCosts.OrderBy(e => e).ToArray();
            List<double> invertedList = new List<double>();

            foreach (var item in sortedList)
            {
                invertedList.Add((double)1 / item);
            }
            double routeCostsSum = invertedList.Sum();

            //Generate a random number between 0 and S.

            //Starting from the top of the population, keep adding the finesses to the partial sum P, till P<S.
            double partialSum = 0;
            
            int j = 0;
            while (partialSum < routeCostsSum)
            {
                partialSum += invertedList[j++];
            }



            //The individual for which P exceeds S is the chosen individual.
            return sortedList;
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
