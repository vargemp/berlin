using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //Deliverer[] DelivererRoutes = FindRandomRoutes(100);
            //List<int> RouteCosts = DelivererRoutes.Select(r => r.GetRouteCost()).ToList();
            Stopwatch sw = new Stopwatch();
            List<Deliverer> Deliverers = FindRandomRoutes(20);
            Console.WriteLine("Srednia na poczatku: " + Deliverers.Average(d => d.GetRouteCost()));
            sw.Start();
            //List<int> RouteCosts = new List<int>();

            //int[] TournamentRoutes = TournamentSelection(3, RouteCosts);
            //List<Deliverer> RouletteRoutes = RouletteSelection(DelivererRoutes);
            for (int i = 0; i < 100; i++)
            {
                RouletteSelection(Deliverers);
                //Console.WriteLine($"Selekcja{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                PmxCrossover(Deliverers);
                //Console.WriteLine($"Krzyzowanie{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                GeneExchangeMutation(Deliverers);
                //Console.WriteLine($"Mutacja{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                Console.WriteLine($"Przebieg {i}: {Deliverers.Average(d => d.GetRouteCost())}");
            }

            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed.TotalSeconds);
            //List<Deliverer> PmxCrossoverDeliverers = PmxCrossover(RouletteRoutes);
            //Console.WriteLine("Srednia po krzyzowaniu: " + PmxCrossoverDeliverers.Average(d => d.GetRouteCost()));

            Console.ReadKey();
        }

        static void RouletteSelection(List<Deliverer> DelivererRoutes)
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

            //List<Deliverer> List2 = new List<Deliverer>();

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
                //List2.Add(List[j].Deliverer);
                DelivererRoutes[i] = List[j].Deliverer;
            }

            
           // return List2;
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

        static void PmxCrossover(List<Deliverer> routes)
        {
            int routesLength = routes.Count;
            List<List<int>> ChildRoutesList = new List<List<int>>();
            List<Deliverer> CrossoverDeliverers = new List<Deliverer>();
            //1 rodzic = i , drugi rodzic = i+1
            for (int i = 0; i < routesLength - 1; i++)
            {
                List<int> parent1 = routes[i].GetCityList();
                List<int> parent2 = routes[i + 1].GetCityList();
                //List<int> parent1 = new List<int>(new int[] { 1,2,3,4,5,6,7,8,9 });
                //List<int> parent2 = new List<int>(new int[] { 9,3,7,8,2,6,5,1,4});

                int p1 = rnd.Next(parent1.Count);
                int p2 = rnd.Next(parent1.Count);
                //int p1 = 3;
                //int p2 = 6;

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
                        //numberToAdd = parent2.IndexOf(parent1[numberToAdd-1]);
                        numberToAdd = parent2[parent1.IndexOf(numberToAdd)];
                    }
                    childCities.Add(numberToAdd);
                }

                //dodajemy do uzupełnionej lewej strony wszystko pomiedzy dwoma punktami
                childCities.AddRange(areaBetweenP1);

                //uzupelniamy czesc tablicy od drugiego przeciecia do konca
                for (int j = p2 + 1; j < parent2.Count; j++)
                {
                    int numberToAdd = parent2[j];
                    while (areaBetweenP1.Contains(numberToAdd))
                    {
                        //numberToAdd = parent2.IndexOf(parent1[numberToAdd - 1]);
                        numberToAdd = parent2[parent1.IndexOf(numberToAdd)];
                    }
                    childCities.Add(numberToAdd);
                }

                //CrossoverDeliverers.Add(new Deliverer(childCities));
                routes[i] = new Deliverer(childCities);
                //ChildRoutesList.Add(childCities);
            }

            //na koniec krzyzowanie ostatniego z pierwszym
            List<int> LastParent = routes[routes.Count - 1].GetCityList(); //parent1
            List<int> FirstParent = routes[0].GetCityList(); //parent2

            int pp1 = rnd.Next(LastParent.Count);
            int pp2 = rnd.Next(FirstParent.Count);

            if (pp1 > pp2)
            {
                int pp3 = pp1;
                pp1 = pp2;
                pp2 = pp3;
            }
            List<int> areaBetweenLastParent = new List<int>();
            List<int> areaBetweenFirstParent = new List<int>();
            for (int j = pp1; j <= pp2; j++)
            {
                areaBetweenLastParent.Add(LastParent[j]);
                areaBetweenFirstParent.Add(FirstParent[j]);
            }

            List<int> childCities2 = new List<int>();

            for (int j = 0; j < pp1; j++)
            {
                int numberToAdd = FirstParent[j];
                while (areaBetweenLastParent.Contains(numberToAdd))
                {
                    numberToAdd = FirstParent[LastParent.IndexOf(numberToAdd)];
                }
                childCities2.Add(numberToAdd);
            }

            childCities2.AddRange(areaBetweenLastParent);

            for (int j = pp2 + 1; j < FirstParent.Count; j++)
            {
                int numberToAdd = FirstParent[j];
                while (areaBetweenLastParent.Contains(numberToAdd))
                {
                    numberToAdd = FirstParent[LastParent.IndexOf(numberToAdd)];
                }
                childCities2.Add(numberToAdd);
            }

            //CrossoverDeliverers.Add(new Deliverer(childCities2));
            routes[routes.Count - 1] = new Deliverer(childCities2);
            //ChildRoutesList.Add(childCities2);



            //int ileListMaDuplikaty = 0;
            //foreach (var item in ChildRoutesList)
            //{
            //    if (item.Count != item.Distinct().Count())
            //    {
            //        ileListMaDuplikaty++;
            //    }
            //}

            //return ChildRoutesList;
            //return CrossoverDeliverers;
        }

        static void GeneExchangeMutation(List<Deliverer> routes)
        {
            int cityCount = routes[0].GetCityList().Count;

            for (int i = 0; i < routes.Count; i++)
            {
                int randomPlace1 = rnd.Next(cityCount-1);
                int randomPlace2 = rnd.Next(cityCount-1);
                while (randomPlace2 == randomPlace1)
                {
                    randomPlace2 = rnd.Next(cityCount-1);
                }

                routes[i].MutateCities(randomPlace1, randomPlace2);
            }
        }

        private static List<Deliverer> FindRandomRoutes(int DeliverersNumber)
        {
            //Deliverer[] DelivererRoutes = new Deliverer[DeliverersNumber];
            List<Deliverer> deliverers = new List<Deliverer>();
            for (int i = 0; i < DeliverersNumber; i++)
            {
                //DelivererRoutes[i] = new Deliverer();
                deliverers.Add(new Deliverer());
            }

            return deliverers;
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
