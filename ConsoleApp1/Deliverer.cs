using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Deliverer
    {
        private List<int> Cities = new List<int>();
        private int RouteCost;
        public Deliverer()
        {
            List<int> FoundCities = new List<int>();
            Random rnd = new Random();

            for (int i = 0; i < 51; i++)
            {
                int randomCity = rnd.Next(0, 52);
                while (Cities.Contains(randomCity))
                {
                    randomCity = rnd.Next(0, 52);
                }
                //.Add(randomCity);
                Cities.Add(randomCity);
            }

            //TODO
            for (int i = 0; i < Cities.Count - 1; i++)
            {
                int cityA;
                int cityB;

                if (Cities[i] < Cities[i + 1])
                {
                    cityA = Cities[i + 1];
                    cityB = Cities[i];
                }
                else
                {
                    cityA = Cities[i];
                    cityB = Cities[i + 1];
                }
                RouteCost += getDistance(cityA, cityB);
            }
            RouteCost += getDistance(Cities.Count - 1, Cities[0]);
        }

        public int GetRouteCost()
        {
            return RouteCost;
        }

        private static int getDistance(int cityA, int cityB)
        {
            string[][] arr = getDistanceArray();


            return int.Parse(arr[cityA][cityB]);
        }

        private static string[][] getDistanceArray()
        {
            string fileName = "D:/berlin52.txt";

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

