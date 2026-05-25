using LocationLibrary;
using System;
using System.Collections.Generic;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА № 10. ЧАСТЬ 1: ИЕРАРХИЯ КЛАССОВ");

        Place[] locations = new Place[10];
        locations[0] = new Place();
        locations[0].RandomInit();

        locations[1] = new Region();
        locations[1].RandomInit();

        locations[2] = new City();
        locations[2].RandomInit();

        locations[3] = new Metropolis();
        locations[3].RandomInit();

        locations[4] = new Address();
        locations[4].RandomInit();

        locations[5] = new City();
        locations[5].RandomInit();

        locations[6] = new Region();
        locations[6].RandomInit();

        locations[7] = new Metropolis();
        locations[7].RandomInit();

        locations[8] = new Address();
        locations[8].RandomInit();

        locations[9] = new Place();
        locations[9].RandomInit();

        LocationManager.ShowAllVirtual(locations);

        LocationManager.ShowAllNonVirtual(locations);

        Console.WriteLine("\nЧАСТЬ 2: ДИНАМИЧЕСКАЯ ИДЕНТИФИКАЦИЯ ТИПОВ");

        QueryEngine.Query1_Statistics(locations);

        QueryEngine.Query2_AveragePopulation(locations, "Уральский");

        QueryEngine.Query3_CitiesFoundedBefore1800(locations);

        Console.WriteLine("\nЧАСТЬ 3: ИНТЕРФЕЙСЫ, СОРТИРОВКА И КЛОНИРОВАНИЕ");

        Console.WriteLine("1. СОРТИРОВКА ПО НАСЕЛЕНИЮ (IComparable):");
        Place[] sortedByPopulation = new Place[locations.Length];
        Array.Copy(locations, sortedByPopulation, locations.Length);
        Array.Sort(sortedByPopulation);

        foreach (Place loc in sortedByPopulation)
        {
            Console.WriteLine($"  {loc.GetType().Name}: {loc.Name} - {loc.Population} чел.");
        }

        Console.WriteLine("\n2. СОРТИРОВКА ПО НАЗВАНИЮ (IComparer):");
        Place[] sortedByName = new Place[locations.Length];
        Array.Copy(locations, sortedByName, locations.Length);
        Array.Sort(sortedByName, new LocationNameComparer());

        foreach (Place loc in sortedByName)
        {
            Console.WriteLine($"  {loc.GetType().Name}: {loc.Name}");
        }

        Console.WriteLine("\n3. БИНАРНЫЙ ПОИСК ЭЛЕМЕНТА:");

        Place searchPlace = new Place("Поисковый объект", 500000);
        searchPlace.RandomInit();

        int index = Array.BinarySearch(sortedByPopulation, searchPlace);
        if (index >= 0)
            Console.WriteLine($"Найден объект с населением ~{searchPlace.Population} в позиции {index}: {sortedByPopulation[index].Name}");
        else
            Console.WriteLine($"Объект с населением ~{searchPlace.Population} не найден");

        Place searchByName = new Place("Поиск по имени", 0);
        searchByName.Name = sortedByName[sortedByName.Length / 2].Name;

        index = Array.BinarySearch(sortedByName, searchByName, new LocationNameComparer());
        if (index >= 0)
            Console.WriteLine($"Найден объект с именем '{searchByName.Name}' в позиции {index}");
        else
            Console.WriteLine($"Объект с именем '{searchByName.Name}' не найден");

        Console.WriteLine("\n--- Работа с интерфейсом IInit ---");
        IInit[] initObjects = new IInit[5];
        initObjects[0] = new City();
        initObjects[1] = new Metropolis();
        initObjects[2] = new Address();
        initObjects[3] = new Region();
        initObjects[4] = new SpecialPlace();

        Console.WriteLine("Инициализация объектов случайными данными:");
        foreach (IInit obj in initObjects)
        {
            obj.RandomInit();
            if (obj is Place p)
                Console.WriteLine($"  {obj.GetType().Name}: {p.Name}");
            else
                Console.WriteLine($"  {obj.GetType().Name}: {obj.ToString()}");
        }

        Console.WriteLine("\n--- Клонирование и поверхностное копирование ---");
        Metropolis original = new Metropolis("Москва", 12500000, "Центральный", "Юрий Долгоруков", 1147, 12, 2561.5);

        Metropolis shallow = original.ShallowCopy();
        Metropolis deep = (Metropolis)original.Clone();

        Console.WriteLine("Оригинальный объект:");
        original.Show();

        Console.WriteLine("\nПоверхностная копия (ShallowCopy):");
        shallow.Show();

        Console.WriteLine("\nГлубокая копия (Clone):");
        deep.Show();
    }
}

public class LocationNameComparer : System.Collections.IComparer
{
    public int Compare(object obj1, object obj2)
    {
        if (obj1 is Place p1 && obj2 is Place p2)
        {
            return string.Compare(p1.Name, p2.Name);
        }
        return 0;
    }
}

public class SpecialPlace : LocationLibrary.IInit
{
    private string designation;
    private Random random = new Random();

    public SpecialPlace()
    {
        designation = "Специальное место";
    }

    public void Init()
    {
        Console.Write("Введите обозначение специального места: ");
        designation = Console.ReadLine();
    }

    public void RandomInit()
    {
        string[] designations = { "Памятник", "Музей", "Парк", "Библиотека", "Храм", "Театр" };
        designation = designations[random.Next(designations.Length)];
    }

    public override string ToString()
    {
        return $"Спецместо: {designation}";
    }
}
