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
            List<Deliverer> Deliverers = FindRandomRoutes(1000);
            Console.WriteLine($"Najlepszy czas na poczatku: {Deliverers.Min(d => d.GetRouteCost())}");
            sw.Start();
            //List<int> RouteCosts = new List<int>();

            //int[] TournamentRoutes = TournamentSelection(3, RouteCosts);
            //List<Deliverer> RouletteRoutes = RouletteSelection(DelivererRoutes);
            for (int i = 0; i < 500; i++)
            {
                RouletteSelection(Deliverers);
                //TournamentSelection(3, Deliverers);
                //Console.WriteLine($"Selekcja{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                //PmxCrossover(Deliverers);
                //Console.WriteLine($"Krzyzowanie{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                GeneExchangeMutation(Deliverers);
                //Console.WriteLine($"Mutacja{i}: {Deliverers.Average(d => d.GetRouteCost())}");
                //Console.WriteLine($"Przebieg {i}: {Deliverers.Average(d => d.GetRouteCost())}");
            }

            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed.TotalSeconds);
            Console.WriteLine($"Najlepszy czas: {Deliverers.Min(d => d.GetRouteCost())}");
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

        static void TournamentSelection(int rozmiarTurnieju, List<Deliverer> DelivererRoutes)
        {
            int liczebnoscPopulacji = DelivererRoutes.Count();
            Random rnd = new Random();
            for (int i = 0; i < liczebnoscPopulacji; i++)
            {
                List<int> foundRandomNumbers = new List<int>();
                int BestRouteOfRandomDeliverer = 100000;
                for (int k = 0; k < rozmiarTurnieju; k++)
                {
                    int randomNum = rnd.Next(liczebnoscPopulacji);
                    while (foundRandomNumbers.Contains(randomNum)){
                        randomNum = rnd.Next(liczebnoscPopulacji);
                    }
                    foundRandomNumbers.Add(randomNum);

                    int RandomDelivererRouteCost = DelivererRoutes[randomNum].GetRouteCost();
                    if ( RandomDelivererRouteCost < BestRouteOfRandomDeliverer)
                    {
                        BestRouteOfRandomDeliverer = RandomDelivererRouteCost;
                    }
                }
                DelivererRoutes[i] = DelivererRoutes.Find(d => d.GetRouteCost() == BestRouteOfRandomDeliverer);
                foundRandomNumbers.Clear();
            }
        }

        static void PmxCrossover(List<Deliverer> routes)
        {
            int routesLength = routes.Count;
            for (int i = 1; i < routesLength - 1; i++)
            {
                routes[i] = CrossoverParents(routes[i], routes[i + 1]);
            }

            //na koniec krzyzowanie ostatniego z pierwszym
            routes[routes.Count - 1] = CrossoverParents(routes[routes.Count - 1], routes[0]);
        }

        static Deliverer CrossoverParents(Deliverer parent1, Deliverer parent2)
        {
            //List<int> parent1Cities = new List<int>(new int[] { 1,2,3,4,5,6,7,8,9 });
            //Deliverer parent1 = new Deliverer(parent1Cities);
            //List<int> parent2Cities = new List<int>(new int[] { 9,3,7,8,2,6,5,1,4});
            //Deliverer parent2 = new Deliverer(parent2Cities);

            int[] childCities = new int[parent1.GetCityList().Count];

            int p1 = rnd.Next(parent1.GetCityList().Count);
            int p2 = rnd.Next(parent1.GetCityList().Count);
            //int p1 = 3;
            //int p2 = 6;
            while (p2 == p1)
            {
                p2 = rnd.Next(parent1.GetCityList().Count);
            }
            if (p1 > p2)
            {
                int p3 = p1;
                p1 = p2;
                p2 = p3;
            }
            //-------wylosowane 2 punkty przeciecia
            
            //liczby pomiedzy dwoma punktami przeciecia ->
            for (int j = p1; j <= p2; j++)
            {
                childCities[j] = parent1.GetCityList()[j];
            }

            int[] areaBetween = new int[(p2-p1)+1];
            Array.Copy(childCities, p1, areaBetween, 0, (p2 - p1)+1);
            

            //uzupelniamy czesc tablicy od lewej do pierwszego przeciecia
            for (int j = 0; j < p1; j++)
            {
                int numberToAdd = parent2.GetCityList()[j];
                while (areaBetween.Contains(numberToAdd))
                {
                    //numberToAdd = parent2.IndexOf(parent1[numberToAdd-1]);
                    numberToAdd = parent2.GetCityList()[parent1.GetCityList().IndexOf(numberToAdd)];
                }
                childCities[j] = numberToAdd;
            }

            //uzupelniamy czesc tablicy od drugiego przeciecia do konca
            for (int j = p2 + 1; j < parent2.GetCityList().Count; j++)
            {
                int numberToAdd = parent2.GetCityList()[j];
                while (areaBetween.Contains(numberToAdd))
                {
                    //numberToAdd = parent2.IndexOf(parent1[numberToAdd - 1]);
                    numberToAdd = parent2.GetCityList()[parent1.GetCityList().IndexOf(numberToAdd)];
                }
                childCities[j] = numberToAdd;
            }

            return (new Deliverer(childCities.ToList()));
        }


        static void GeneExchangeMutation(List<Deliverer> routes)
        {
            int cityCount = routes[0].GetCityList().Count;
            for (int i = 0; i < cityCount; i++)
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
