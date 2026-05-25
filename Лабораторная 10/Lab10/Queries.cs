using LocationLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace LocationLibrary
{
    public class QueryEngine
    {
        public static void Query1_Statistics(Place[] locations)
        {
            int placeCount = 0, regionCount = 0, cityCount = 0, metropolisCount = 0, addressCount = 0;
            foreach (Place loc in locations)
            {
                if (loc is Metropolis) metropolisCount++;
                else if (loc is City) cityCount++;
                else if (loc is Region) regionCount++;
                else if (loc is Address) addressCount++;
                else if (loc is Place) placeCount++;
            }

            Console.WriteLine("\n=== Запрос 1: Статистика по типам ===");
            Console.WriteLine($"Всего объектов: {locations.Length}");
            Console.WriteLine($"Place: {placeCount}");
            Console.WriteLine($"Region: {regionCount}");
            Console.WriteLine($"City: {cityCount}");
            Console.WriteLine($"Metropolis: {metropolisCount}");
            Console.WriteLine($"Address: {addressCount}");
        }

        public static void Query2_AveragePopulation(Place[] locations, string federalDistrict)
        {
            double avgPopulation = 0;
            foreach (Place loc in locations)
            {
                avgPopulation += loc.Population;
            }
            avgPopulation /= locations.Length;
            Console.WriteLine($"\n=== Запрос 2: Среднее население = {avgPopulation:F0} ===");
        }

        public static void Query3_CitiesFoundedBefore1800(Place[] locations)
        {
            Console.WriteLine("\n=== Запрос 3: Города, основанные до 1800 года ===");
            bool found = false;
            foreach (Place loc in locations)
            {
                if (loc is City city && city.FoundedYear < 1800)
                {
                    city.Show();
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Города не найдены");
        }
    }
}
