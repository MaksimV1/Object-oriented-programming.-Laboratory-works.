using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LocationLibrary;

namespace Lab14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЧАСТЬ 1 ===");

            List<SortedDictionary<string, Place>> continent = new List<SortedDictionary<string, Place>>();

            SortedDictionary<string, Place> italy = new SortedDictionary<string, Place>();
            SortedDictionary<string, Place> spain = new SortedDictionary<string, Place>();
            SortedDictionary<string, Place> france = new SortedDictionary<string, Place>();

            FillCountry(italy, "Italy", 8);
            FillCountry(spain, "Spain", 6);
            FillCountry(france, "France", 7);

            continent.Add(italy);
            continent.Add(spain);
            continent.Add(france);

            Console.WriteLine("Континент сформирован. Количество стран: " + continent.Count);

            var allPlaces = continent.SelectMany(c => c.Values).ToList();
            Console.WriteLine("Количество мест: " + allPlaces.Count);
            foreach (var place in allPlaces)
            {
                place.Show();
            }

            Console.WriteLine("\n--- Where (население > 500000) ---");
            var whereLinq = (from country in continent
                             from place in country.Values
                             where place.Population > 500000
                             select place).ToList();
            Console.WriteLine($"LINQ: найдено {whereLinq.Count} мест");

            var whereExt = continent.SelectMany(c => c.Values).Where(p => p.Population > 500000);
            foreach (var place in whereExt)
            {
                place.Show();
            }
            Console.WriteLine($"Методы расширения: найдено {whereExt.Count()} мест");
            foreach (var place in whereLinq)
            {
                place.Show();
            }

            Console.WriteLine("\n--- Операции над множествами ---");
            var italyPlaces = italy.Values.ToList();
            var spainPlaces = spain.Values.ToList();
            Console.WriteLine($"1 множество: {italyPlaces.Count} элементов");
            Console.WriteLine($"2 множество: {spainPlaces.Count} элементов");

            var union = italyPlaces.Union(spainPlaces).ToList();
            var except = italyPlaces.Except(spainPlaces).ToList();
            var intersect = italyPlaces.Intersect(spainPlaces).ToList();

            Console.WriteLine($"Union: {union.Count} элементов");
            foreach (var place in union)
            {
                place.Show();
            }
            Console.WriteLine();

            Console.WriteLine($"Except: {except.Count} элементов");
            foreach (var place in except)
            {
                place.Show();
            }
            Console.WriteLine();

            Console.WriteLine($"Intersect: {intersect.Count} элементов");
            foreach (var place in intersect)
            {
                place.Show();
            }
            Console.WriteLine();

            Console.WriteLine("\n--- Агрегирование данных ---");
            var totalPopulation = allPlaces.Sum(p => p.Population);
            var maxPopulation = allPlaces.Max(p => p.Population);
            var minPopulation = allPlaces.Min(p => p.Population);
            var avgPopulation = allPlaces.Average(p => p.Population);
            Console.WriteLine($"Суммарное население: {totalPopulation}");
            Console.WriteLine($"Максимальное население: {maxPopulation}");
            Console.WriteLine($"Минимальное население: {minPopulation}");
            Console.WriteLine($"Среднее население: {avgPopulation:F2}");

            Console.WriteLine("\n--- Группировка данных ---");
            var groupByType = allPlaces.GroupBy(p => p.GetType().Name);
            foreach (var group in groupByType)
            {
                Console.WriteLine($"{group.Key}: {group.Count()} элементов");
                foreach(var place in group)
                {
                    place.Show();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n--- e)\tПолучение нового типа (let) ---");
            var metropolisQuery = from place in allPlaces
                                  where place is Metropolis
                                  let metropolis = place as Metropolis
                                  let density = metropolis.Population / metropolis.AreaKm2
                                  select new { Name = metropolis.Name, Density = density };
            foreach (var item in metropolisQuery)
            {
                Console.WriteLine($"{item.Name}: плотность {item.Density:F2} чел/км²");
            }

            Console.WriteLine("\n--- Join: города и области по федеральному округу ---");
            var cities = allPlaces.OfType<City>().ToList();
            var regions = allPlaces.OfType<LocationLibrary.Region>().ToList();
            var joined = from city in cities
                         join region in regions on city.FederalDistrict equals region.FederalDistrict
                         select new { City = city.Name, Region = region.Name, District = city.FederalDistrict };
            foreach (var item in joined)
            {
                Console.WriteLine($"{item.City} (город) - {item.Region} (область) : округ {item.District}");
            }

            Console.WriteLine("\n--- Замер времени выполнения Where ---");
            var bigCollection = allPlaces.ToList();
            Stopwatch sw = Stopwatch.StartNew();
            var resultLinq = (from country in continent
                             from place in country.Values
                             where place.Population > 500000
                             select place).ToList();
            sw.Stop();
            Console.WriteLine($"a) С использованием LINQ запросов: {sw.ElapsedTicks} тиков");

            sw.Restart();
            var resultExt = continent.SelectMany(c => c.Values).Where(p => p.Population > 500000).ToList();
            sw.Stop();
            Console.WriteLine($"b) С использованием методов расширения: {sw.ElapsedTicks} тиков");

            sw.Restart();
            var resultFor = new List<Place>();
            for (int i = 0; i < bigCollection.Count; i++)
            {
                if (bigCollection[i].Population > 500000) resultFor.Add(bigCollection[i]);
            }
            sw.Stop();
            Console.WriteLine($"Цикл for: {sw.ElapsedTicks} тиков");
            Console.WriteLine("Вывод: производительность обычного цикла for значительно превосходит LINQ запросы и методы расширения.");



            Console.WriteLine("\n=== ЧАСТЬ 2 ===");

            MyNewCollection<Place> myCollection = new MyNewCollection<Place>("Европа");
            myCollection.AddRandom(10);
            Console.WriteLine($"Создана коллекция '{myCollection.Name}', элементов: {myCollection.Count}");

            Console.WriteLine("\n--- Метод Filter (выборка по условию) ---");
            var filtered = myCollection.Filter(p => p.Population > 500000).ToList();
            Console.WriteLine($"Население > 500000: {filtered.Count} элементов");
            foreach (var p in filtered)
            {
                p.Show();
            }

            Console.WriteLine("\n--- Среднее арифметическое ---");
            double avgPop = myCollection.AveragePopulation(p => p.Population);
            Console.WriteLine($"Среднее население: {avgPop:F2}");

            Console.WriteLine("\n--- Сортировка по алфавиту ---");
            var sortedByName = myCollection.OrderByAlphabet(p => p.Name);
            foreach (var p in sortedByName)
            {
                p.Show();
            }
        }

        static void FillCountry(SortedDictionary<string, Place> country, string countryName, int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                Place p;
                int type = rnd.Next(5);
                switch (type)
                {
                    case 0: p = new Place(); break;
                    case 1: p = new LocationLibrary.Region(); break;
                    case 2: p = new City(); break;
                    case 3: p = new Metropolis(); break;
                    default: p = new Address(); break;
                }
                p.RandomInit();
                string key = $"{countryName}_{p.Name}_{i}";
                country[key] = p;
            }
        }
    }
}