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
            
            for (int i = 0; i <= 51; i++)
            {
                int randomCity = rnd.Next(0, 52);
                while (Cities.Contains(randomCity))
                {
                    randomCity = rnd.Next(0, 52);
                }
                Cities.Add(randomCity);
            }
            
            for (int i = 0; i < Cities.Count - 1; i++)
            {
                RouteCost += getDistance(Cities[i], Cities[i + 1]);
            }
            RouteCost += getDistance(Cities.Count - 1, Cities[0]);
        }

        public int GetRouteCost()
        {
            return RouteCost;
        }

        public List<int> GetCityList()
        {
            return Cities;
        }

        private int getDistance(int cityA, int cityB)
        {
            int[,] arr = getDistanceArray();
            return arr[cityA,cityB];
        }

        private static int[,] getDistanceArray()
        {
            string fileName = "berlin52.txt";

            var lines = File.ReadAllLines(fileName);
            string[][] array = new string[lines.Length-1][];
            int[,] array2 = new int[lines.Length-1, lines.Length - 1];
            for (var i = 1; i < lines.Length; i += 1)
            {
                var line = lines[i].Remove(lines[i].Length-1,1);//wycina ostatni element "" z wiersza
                //array[i - 1] = line.Split(' ');
                string [] splittedLine = line.Split(' ');
                for (int j = 0; j < splittedLine.Length; j++)
                {
                    array2[i - 1, j] = int.Parse(splittedLine[j]);
                }
                
            }
            for (int i = 0; i < 52; i++)
            {
                for (int j = i + 1; j < 52; j++)
                {
                    array2[i, j] = array2[j, i];
                }
            }

            return array2;
        }
    }
}

