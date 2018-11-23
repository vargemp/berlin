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

            //int[] TournamentRoutes = TournamentSelection(3, RouteCosts);
            List<Deliverer> RouletteRoutes = RouletteSelection(DelivererRoutes);
            PmxCrossover(RouletteRoutes);


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

        static List<List<int>> PmxCrossover(List<Deliverer> routes)
        {
            int routesLength = routes.Count;
            List<List<int>> ChildRoutesList = new List<List<int>>();
            //1 rodzic = i , drugi rodzic = i+1
            for (int i = 0; i < routesLength - 1; i++)
            {
                //List<int> parent1 = routes[i].GetCityList();
                //List<int> parent2 = routes[i + 1].GetCityList();
                List<int> parent1 = new List<int>(new int[] { 1,2,3,4,5,6,7,8,9 });
                List<int> parent2 = new List<int>(new int[] { 9,3,7,8,2,6,5,1,4});

                //int p1 = rnd.Next(parent1.Count);
                //int p2 = rnd.Next(parent1.Count);
                int p1 = 3;
                int p2 = 6;

                if (p1 > p2)
                {
                    int p3 = p1;
                    p1 = p2;
                    p2 = p3;
                }
                //-------wylosowane 2 punkty przeciecia

                List<int> areaBetweenP1 = new List<int>();
                List<int> areaBetweenP2 = new List<int>();
                //liczby pomiedzy dwoma punktami przeciecia ->
                for (int j = p1; j <= p2; j++)
                {
                    areaBetweenP1.Add(parent1[j]);
                    areaBetweenP2.Add(parent2[j]);
                }

                List<int> childCities = new List<int>();

                //uzupelniamy czesc tablicy od lewej do pierwszego przeciecia
                for (int j = 0; j < p1; j++)
                {
                    int numberToAdd = parent2[j];
                    while (areaBetweenP1.Contains(numberToAdd))
                    {
                        //indexToAdd = parent1.IndexOf(parent2[indexToAdd]);
                        numberToAdd = parent2.IndexOf(parent1[numberToAdd-1]);
                    }
                    childCities.Add(numberToAdd);
                }

                //dodajemy do uzupełnionej lewej strony wszystko pomiedzy dwoma punktami
                childCities.AddRange(areaBetweenP1);
                

                ChildRoutesList.Add(childCities);
            }

            int ileListMaDuplikaty = 0;
            foreach (var item in ChildRoutesList)
            {
                if (item.Count != item.Distinct().Count())
                {
                    ileListMaDuplikaty++;
                }
            }

            return ChildRoutesList;
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
