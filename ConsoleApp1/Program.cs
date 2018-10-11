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
            Console.WriteLine(getDistance(2,1));

            Console.ReadKey();
        }

        private static string getDistance(int cityA, int cityB)
        {
            string[][] arr = getDistanceArray();


            return arr[cityA][cityB];
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
                Array.Resize(ref array[i-1], array[i-1].Length - 1);
            }

            return array;
        }
        
    }
}
