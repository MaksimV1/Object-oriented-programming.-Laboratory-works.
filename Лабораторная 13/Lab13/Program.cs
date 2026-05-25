using Lab13;
using LocationLibrary;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;


namespace TreeCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            MyNewCollection<Place> collection1 = new MyNewCollection<Place>("Коллекция 1");
            MyNewCollection<Place> collection2 = new MyNewCollection<Place>("Коллекция 2");

            Journal journal1 = new Journal();
            Journal journal2 = new Journal();

            collection1.CollectionCountChanged += new CollectionHandler(journal1.HandleCountChanged);
            collection1.CollectionReferenceChanged += new CollectionHandler(journal1.HandleReferenceChanged);

            collection1.CollectionReferenceChanged += new CollectionHandler(journal2.HandleReferenceChanged);
            collection2.CollectionReferenceChanged += new CollectionHandler(journal2.HandleReferenceChanged);

            Console.WriteLine("=== Добавление случайных элементов в collection1 ===");
            collection1.AddRandom(2);

            Console.WriteLine("\n=== Добавление массива элементов в collection2 ===");
            Place[] places = new Place[]
            {
                new City("Милан", 1352000, "Ломбардия", "Кельты", 400),
                new Metropolis("Рим", 2873000, "Лацио", "Ромул", 753, 15, 1285.0),
                new LocationLibrary.Region("Тоскана", 3747000, "Центральный")
            };
            collection2.Add(places);

            Console.WriteLine("\n=== Удаление элемента из collection1 по индексу 0 ===");
            collection1.Remove(0);

            Console.WriteLine("\n=== Замена элемента в collection1 по индексу 0 ===");
            if (collection1.Count > 0)
            {
                collection1[0] = new City("Флоренция", 382000, "Тоскана", "Юлий Цезарь", 59);
            }

            Console.WriteLine("\n=== Замена элемента в collection2 по индексу 1 ===");
            if (collection2.Count > 1)
            {
                collection2[1] = new Metropolis("Неаполь", 967000, "Кампания", "Греки", 600, 10, 117.0);
            }

            Console.WriteLine("\n=== Журнал 1 (подписан на оба события collection1) ===");
            journal1.Print();

            Console.WriteLine("\n=== Журнал 2 (подписан на CollectionReferenceChanged обеих коллекций) ===");
            journal2.Print();
        }
    }
}
