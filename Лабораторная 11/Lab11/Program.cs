using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LocationLibrary;

class Program
{
    static ArrayList arrayList = new ArrayList();
    static Dictionary<string, Place> dictionary = new Dictionary<string, Place>();
    static TestCollections testCollections;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Задание 1 - Работа с ArrayList");
            Console.WriteLine("2. Задание 2 - Работа с Dictionary<K,T>");
            Console.WriteLine("3. Задание 3 - Тестирование коллекций");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите задание: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Task1Menu();
                    break;
                case "2":
                    Task2Menu();
                    break;
                case "3":
                    Task3Menu();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void Task1Menu()
    {
        while (true)
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 1 - ArrayList ===");
            Console.WriteLine("1. Добавить объект");
            Console.WriteLine("2. Добавить несколько объектов (случайно)");
            Console.WriteLine("3. Удалить объект");
            Console.WriteLine("4. Выполнить запросы");
            Console.WriteLine("5. Перебор элементов (foreach)");
            Console.WriteLine("6. Клонировать коллекцию");
            Console.WriteLine("7. Отсортировать коллекцию");
            Console.WriteLine("8. Найти элемент");
            Console.WriteLine("9. Показать все объекты");
            Console.WriteLine("10. Очистить коллекцию");
            Console.WriteLine("11. Вернуться в главное меню");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddObjectToArrayList();
                    break;
                case "2":
                    AddMultipleObjectsToArrayList();
                    break;
                case "3":
                    RemoveObjectFromArrayList();
                    break;
                case "4":
                    ExecuteQueriesArrayList();
                    break;
                case "5":
                    IterateArrayList();
                    break;
                case "6":
                    CloneArrayList();
                    break;
                case "7":
                    SortArrayList();
                    break;
                case "8":
                    SearchInArrayList();
                    break;
                case "9":
                    ShowAllArrayList();
                    break;
                case "10":
                    ClearArrayList();
                    break;
                case "11":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void AddObjectToArrayList()
    {
        Console.WriteLine("\nВыберите тип объекта:");
        Console.WriteLine("1. Place (Место)");
        Console.WriteLine("2. Region (Область)");
        Console.WriteLine("3. City (Город)");
        Console.WriteLine("4. Metropolis (Мегаполис)");
        Console.WriteLine("5. Address (Адрес)");
        Console.Write("Ваш выбор: ");

        string typeChoice = Console.ReadLine();
        Place obj;

        switch (typeChoice)
        {
            case "1":
                obj = new Place();
                break;
            case "2":
                obj = new Region();
                break;
            case "3":
                obj = new City();
                break;
            case "4":
                obj = new Metropolis();
                break;
            case "5":
                obj = new Address();
                break;
            default:
                Console.WriteLine("Неверный выбор!");
                return;
        }

        Console.WriteLine("\nКак создать объект?");
        Console.WriteLine("1. Вручную");
        Console.WriteLine("2. Случайно");
        Console.Write("Ваш выбор: ");

        string createChoice = Console.ReadLine();

        if (createChoice == "1")
            obj.Init();
        else if (createChoice == "2")
            obj.RandomInit();
        else
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        arrayList.Add(obj);
        Console.WriteLine("Объект добавлен!");
    }

    static void AddMultipleObjectsToArrayList()
    {
        Console.Write("\nВведите количество объектов для добавления: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Некорректное количество!");
            return;
        }

        Random random = new Random();
        int addedCount = 0;

        for (int i = 0; i < count; i++)
        {
            int type = random.Next(1, 6);
            Place obj = null;

            switch (type)
            {
                case 1:
                    obj = new Place();
                    break;
                case 2:
                    obj = new Region();
                    break;
                case 3:
                    obj = new City();
                    break;
                case 4:
                    obj = new Metropolis();
                    break;
                case 5:
                    obj = new Address();
                    break;
            }

            try
            {
                obj.RandomInit();
                arrayList.Add(obj);
                addedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании объекта: {ex.Message}");
            }
        }

        Console.WriteLine($"Добавлено {addedCount} из {count} объектов.");
    }

    static void RemoveObjectFromArrayList()
    {
        if (arrayList.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        ShowAllArrayList();
        Console.Write("Введите индекс объекта для удаления (0-" + (arrayList.Count - 1) + "): ");

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < arrayList.Count)
        {
            arrayList.RemoveAt(index);
            Console.WriteLine("Объект удален!");
        }
        else
        {
            Console.WriteLine("Неверный индекс!");
        }
    }

    static void ExecuteQueriesArrayList()
    {
        if (arrayList.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n=== ВЫБОР ЗАПРОСА ===");
            Console.WriteLine("1. Статистика по типам объектов");
            Console.WriteLine("2. Среднее население");
            Console.WriteLine("3. Города, основанные до 1800 года");
            Console.WriteLine("4. Вернуться в меню ArrayList");
            Console.Write("Выберите запрос: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Query1_StatisticsArrayList();
                    break;
                case "2":
                    Query2_AveragePopulationArrayList();
                    break;
                case "3":
                    Query3_CitiesFoundedBefore1800ArrayList();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void Query1_StatisticsArrayList()
    {
        int placeCount = 0, regionCount = 0, cityCount = 0, metropolisCount = 0, addressCount = 0;

        foreach (object obj in arrayList)
        {
            if (obj is Metropolis) metropolisCount++;
            else if (obj is City) cityCount++;
            else if (obj is Region) regionCount++;
            else if (obj is Address) addressCount++;
            else if (obj is Place) placeCount++;
        }

        Console.WriteLine("\n=== Запрос 1: Статистика по типам ===");
        Console.WriteLine($"Всего объектов: {arrayList.Count}");
        Console.WriteLine($"Place: {placeCount}");
        Console.WriteLine($"Region: {regionCount}");
        Console.WriteLine($"City: {cityCount}");
        Console.WriteLine($"Metropolis: {metropolisCount}");
        Console.WriteLine($"Address: {addressCount}");
    }
    static void Query2_AveragePopulationArrayList()
    {
        double avgPopulation = 0;
        foreach (Place obj in arrayList)
        {
            avgPopulation += obj.Population;
        }
        avgPopulation /= arrayList.Count;
        Console.WriteLine($"\n=== Запрос 2: Среднее население = {avgPopulation:F0} ===");
    }

    static void Query3_CitiesFoundedBefore1800ArrayList()
    {
        Console.WriteLine("\n=== Запрос 3: Города, основанные до 1800 года ===");
        bool found = false;
        foreach (object obj in arrayList)
        {
            if (obj is City city && city.FoundedYear < 1800)
            {
                city.Show();
                found = true;
            }
        }
        if (!found) Console.WriteLine("Города не найдены");
    }

    static void IterateArrayList()
    {
        Console.WriteLine("\n=== Перебор элементов коллекции (foreach) ===");
        foreach (Place obj in arrayList)
        {
            obj.Show();
        }
    }

    static void CloneArrayList()
    {
        ArrayList cloned = (ArrayList)arrayList.Clone();
        Console.WriteLine("Коллекция клонирована!");
        Console.WriteLine($"Оригинал: {arrayList.Count} элементов");
        Console.WriteLine($"Клон: {cloned.Count} элементов");
    }

    static void SortArrayList()
    {
        if (arrayList.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        try
        {
            ArrayList tempList = new ArrayList();
            foreach (Place obj in arrayList)
            {
                tempList.Add(obj);
            }

            tempList.Sort();

            Console.WriteLine("\n=== Коллекция после сортировки по населению ===");
            foreach (Place obj in tempList)
            {
                Console.WriteLine($"{obj.Name}: {obj.Population} чел.");
            }

            Console.Write("\nСохранить отсортированную коллекцию? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                arrayList = tempList;
                Console.WriteLine("Коллекция сохранена!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сортировке: {ex.Message}");
        }
    }

    static void SearchInArrayList()
    {
        if (arrayList.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        Console.Write("Введите название для поиска: ");
        string searchName = Console.ReadLine();

        bool found = false;
        for (int i = 0; i < arrayList.Count; i++)
        {
            Place obj = (Place)arrayList[i];
            if (obj.Name.Equals(searchName))
            {
                Console.WriteLine($"Объект найден (индекс {i}):");
                obj.Show();
                found = true;
                break;
            }
        }

        if (!found)
            Console.WriteLine("Объект не найден!");
    }

    static void ShowAllArrayList()
    {
        Console.WriteLine($"\n=== Все объекты в ArrayList ({arrayList.Count}) ===");
        for (int i = 0; i < arrayList.Count; i++)
        {
            Console.Write($"[{i}] ");
            ((Place)arrayList[i]).Show();
        }
    }

    static void ClearArrayList()
    {
        if (arrayList.Count == 0)
        {
            Console.WriteLine("Коллекция уже пуста!");
            return;
        }

        Console.Write($"Вы уверены, что хотите удалить все {arrayList.Count} элементов? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            arrayList.Clear();
            Console.WriteLine("Коллекция очищена!");
        }
        else
        {
            Console.WriteLine("Операция отменена");
        }
    }

    static void Task2Menu()
    {
        while (true)
        {
            Console.WriteLine("\n=== ЗАДАНИЕ 2 - Dictionary<K,T> ===");
            Console.WriteLine("1. Добавить объект");
            Console.WriteLine("2. Добавить несколько объектов (случайно)");
            Console.WriteLine("3. Удалить объект");
            Console.WriteLine("4. Выполнить запросы");
            Console.WriteLine("5. Перебор элементов (foreach)");
            Console.WriteLine("6. Клонировать коллекцию");
            Console.WriteLine("7. Отсортировать и показать");
            Console.WriteLine("8. Найти элемент");
            Console.WriteLine("9. Показать все объекты");
            Console.WriteLine("10. Очистить коллекцию");
            Console.WriteLine("11. Вернуться в главное меню");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddObjectToDictionary();
                    break;
                case "2":
                    AddMultipleObjectsToDictionary();
                    break;
                case "3":
                    RemoveObjectFromDictionary();
                    break;
                case "4":
                    ExecuteQueriesDictionary();
                    break;
                case "5":
                    IterateDictionary();
                    break;
                case "6":
                    CloneDictionary();
                    break;
                case "7":
                    SortAndShowDictionary();
                    break;
                case "8":
                    SearchInDictionary();
                    break;
                case "9":
                    ShowAllDictionary();
                    break;
                case "10":
                    ClearDictionary();
                    break;
                case "11":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void AddObjectToDictionary()
    {
        Console.WriteLine("\nВыберите тип объекта:");
        Console.WriteLine("1. Place (Место)");
        Console.WriteLine("2. Region (Область)");
        Console.WriteLine("3. City (Город)");
        Console.WriteLine("4. Metropolis (Мегаполис)");
        Console.WriteLine("5. Address (Адрес)");
        Console.Write("Ваш выбор: ");

        string typeChoice = Console.ReadLine();
        Place obj = null;

        switch (typeChoice)
        {
            case "1":
                obj = new Place();
                break;
            case "2":
                obj = new Region();
                break;
            case "3":
                obj = new City();
                break;
            case "4":
                obj = new Metropolis();
                break;
            case "5":
                obj = new Address();
                break;
            default:
                Console.WriteLine("Неверный выбор!");
                return;
        }

        Console.WriteLine("\nКак создать объект?");
        Console.WriteLine("1. Вручную");
        Console.WriteLine("2. Случайно");
        Console.Write("Ваш выбор: ");

        string createChoice = Console.ReadLine();

        if (createChoice == "1")
            obj.Init();
        else if (createChoice == "2")
            obj.RandomInit();
        else
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        string key = $"{obj.GetType().Name}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        dictionary.Add(key, obj);
        Console.WriteLine($"Объект добавлен с ключом: {key}");
    }

    static void AddMultipleObjectsToDictionary()
    {
        Console.Write("\nВведите количество объектов для добавления: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Некорректное количество!");
            return;
        }

        Random random = new Random();
        int addedCount = 0;

        for (int i = 0; i < count; i++)
        {
            int type = random.Next(1, 6);
            Place obj = null;

            switch (type)
            {
                case 1:
                    obj = new Place();
                    break;
                case 2:
                    obj = new Region();
                    break;
                case 3:
                    obj = new City();
                    break;
                case 4:
                    obj = new Metropolis();
                    break;
                case 5:
                    obj = new Address();
                    break;
            }

            try
            {
                obj.RandomInit();
                string key = $"{obj.GetType().Name}_{Guid.NewGuid().ToString().Substring(0, 8)}_{i}";
                dictionary.Add(key, obj);
                addedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании объекта: {ex.Message}");
            }
        }

        Console.WriteLine($"Добавлено {addedCount} из {count} объектов.");
    }

    static void RemoveObjectFromDictionary()
    {
        if (dictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        ShowAllDictionary();
        Console.Write("Введите ключ объекта для удаления: ");
        string key = Console.ReadLine();

        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
            Console.WriteLine("Объект удален!");
        }
        else
        {
            Console.WriteLine("Ключ не найден!");
        }
    }

    static void ExecuteQueriesDictionary()
    {
        if (dictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n=== ВЫБОР ЗАПРОСА ===");
            Console.WriteLine("1. Статистика по типам объектов");
            Console.WriteLine("2. Среднее население");
            Console.WriteLine("3. Города, основанные до 1800 года");
            Console.WriteLine("4. Вернуться в меню Dictionary");
            Console.Write("Выберите запрос: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Query1_StatisticsDictionary();
                    break;
                case "2":
                    Query2_AveragePopulationDictionary();
                    break;
                case "3":
                    Query3_CitiesFoundedBefore1800Dictionary();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void Query1_StatisticsDictionary()
    {
        int placeCount = 0, regionCount = 0, cityCount = 0, metropolisCount = 0, addressCount = 0;

        foreach (var kvp in dictionary)
        {
            if (kvp.Value is Metropolis) metropolisCount++;
            else if (kvp.Value is City) cityCount++;
            else if (kvp.Value is Region) regionCount++;
            else if (kvp.Value is Address) addressCount++;
            else if (kvp.Value is Place) placeCount++;
        }

        Console.WriteLine("\n=== Запрос 1: Статистика по типам ===");
        Console.WriteLine($"Всего объектов: {dictionary.Count}");
        Console.WriteLine($"Place: {placeCount}");
        Console.WriteLine($"Region: {regionCount}");
        Console.WriteLine($"City: {cityCount}");
        Console.WriteLine($"Metropolis: {metropolisCount}");
        Console.WriteLine($"Address: {addressCount}");
    }

    static void Query2_AveragePopulationDictionary()
    {
        double avgPopulation = 0;
        foreach (var kvp in dictionary)
        {
            avgPopulation += kvp.Value.Population;
        }
        avgPopulation /= dictionary.Count;
        Console.WriteLine($"\n=== Запрос 2: Среднее население = {avgPopulation:F0} ===");
    }

    static void Query3_CitiesFoundedBefore1800Dictionary()
    {
        Console.WriteLine("\n=== Запрос 3: Города, основанные до 1800 года ===");
        bool found = false;
        foreach (var kvp in dictionary)
        {
            if (kvp.Value is City city && city.FoundedYear < 1800)
            {
                Console.Write($"Ключ: {kvp.Key} - ");
                city.Show();
                found = true;
            }
        }
        if (!found) Console.WriteLine("Города не найдены");
    }

    static void IterateDictionary()
    {
        Console.WriteLine("\n=== Перебор элементов коллекции (foreach) ===");
        foreach (var kvp in dictionary)
        {
            Console.Write($"Ключ: {kvp.Key} - ");
            kvp.Value.Show();
        }
    }

    static void CloneDictionary()
    {
        Dictionary<string, Place> cloned = new Dictionary<string, Place>(dictionary);
        Console.WriteLine("Коллекция клонирована!");
        Console.WriteLine($"Оригинал: {dictionary.Count} элементов");
        Console.WriteLine($"Клон: {cloned.Count} элементов");
    }

    static void SortAndShowDictionary()
    {
        if (dictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        var sortedByKey = new SortedDictionary<string, Place>(dictionary);

        Console.WriteLine("\n=== Коллекция после сортировки по ключу ===");
        foreach (var kvp in sortedByKey)
        {
            Console.WriteLine($"Ключ: {kvp.Key} -> {kvp.Value.Name} ({kvp.Value.Population} чел.)");
        }

        Console.WriteLine("\n=== Коллекция после сортировки по населению ===");
        var sortedByValue = dictionary.OrderBy(x => x.Value.Population).ToList();

        foreach (var kvp in sortedByValue)
        {
            Console.WriteLine($"{kvp.Value.Name}: {kvp.Value.Population} чел. (Ключ: {kvp.Key})");
        }
    }

    static void SearchInDictionary()
    {
        if (dictionary.Count == 0)
        {
            Console.WriteLine("Коллекция пуста!");
            return;
        }

        Console.WriteLine("1. Поиск по ключу");
        Console.WriteLine("2. Поиск по названию");
        Console.Write("Выберите тип поиска: ");

        string searchType = Console.ReadLine();

        if (searchType == "1")
        {
            Console.Write("Введите ключ для поиска: ");
            string key = Console.ReadLine();

            if (dictionary.TryGetValue(key, out Place value))
            {
                Console.WriteLine("Объект найден:");
                value.Show();
            }
            else
            {
                Console.WriteLine("Объект не найден!");
            }
        }
        else if (searchType == "2")
        {
            Console.Write("Введите название для поиска: ");
            string searchName = Console.ReadLine();

            bool found = false;
            foreach (var kvp in dictionary)
            {
                if (kvp.Value.Name.Equals(searchName))
                {
                    Console.WriteLine("Объект найден:");
                    Console.Write($"Ключ: {kvp.Key} - ");
                    kvp.Value.Show();
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("Объект не найден!");
        }
    }

    static void ShowAllDictionary()
    {
        Console.WriteLine($"\n=== Все объекты в Dictionary ({dictionary.Count}) ===");
        foreach (var kvp in dictionary)
        {
            Console.Write($"Ключ: {kvp.Key} - ");
            kvp.Value.Show();
        }
    }

    static void ClearDictionary()
    {
        if (dictionary.Count == 0)
        {
            Console.WriteLine("Коллекция уже пуста!");
            return;
        }

        Console.Write($"Вы уверены, что хотите удалить все {dictionary.Count} элементов? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            dictionary.Clear();
            Console.WriteLine("Коллекция очищена!");
        }
        else
        {
            Console.WriteLine("Операция отменена");
        }
    }

    static void Task3Menu()
    {
        Console.WriteLine("\n=== ЗАДАНИЕ 3 - Тестирование коллекций ===");

        if (testCollections == null)
        {
            Console.WriteLine("Инициализация тестовых коллекций с 1000 элементов...");
            testCollections = new TestCollections(1000);
            Console.WriteLine("Коллекции успешно созданы!");
        }

        while (true)
        {
            Console.WriteLine("\n1. Измерить время поиска");
            Console.WriteLine("2. Добавить элемент");
            Console.WriteLine("3. Удалить элемент");
            Console.WriteLine("4. Показать информацию о коллекциях");
            Console.WriteLine("5. Вернуться в главное меню");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    MeasureSearchTime();
                    break;
                case "2":
                    AddElementToTestCollections();
                    break;
                case "3":
                    RemoveElementFromTestCollections();
                    break;
                case "4":
                    ShowTestCollectionsInfo();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void MeasureSearchTime()
    {
        if (testCollections.Collection1.Count == 0)
        {
            Console.WriteLine("Коллекции пусты!");
            return;
        }

        Console.WriteLine("\n=== Измерение времени поиска ===");

        City first = testCollections.Collection1[0];
        City middle = testCollections.Collection1[testCollections.Collection1.Count / 2];
        City last = testCollections.Collection1[testCollections.Collection1.Count - 1];
        City notInCollection = testCollections.GenerateCityNotInCollection();

        Console.WriteLine("Тестовые элементы:");
        Console.WriteLine($"1. Первый: {first}");
        Console.WriteLine($"2. Средний: {middle}");
        Console.WriteLine($"3. Последний: {last}");
        Console.WriteLine($"4. Не в коллекции: {notInCollection}");

        City firstCopy = new City(first.Name, first.Population, first.FederalDistrict, first.FounderName, first.FoundedYear);
        City middleCopy = new City(middle.Name, middle.Population, middle.FederalDistrict, middle.FounderName, middle.FoundedYear);
        City lastCopy = new City(last.Name, last.Population, last.FederalDistrict, last.FounderName, last.FoundedYear);

        Place firstBaseCopy = firstCopy.BasePlace;
        Place middleBaseCopy = middleCopy.BasePlace;
        Place lastBaseCopy = lastCopy.BasePlace;

        string firstString = firstCopy.ToString();
        string middleString = middleCopy.ToString();
        string lastString = lastCopy.ToString();
        string notInCollectionString = notInCollection.ToString();

        Console.WriteLine("\n=== Поиск в List<City> (Contains) ===");
        MeasureListCityContains(firstCopy, "Первый элемент");
        MeasureListCityContains(middleCopy, "Средний элемент");
        MeasureListCityContains(lastCopy, "Последний элемент");
        MeasureListCityContains(notInCollection, "Не в коллекции");

        Console.WriteLine("\n=== Поиск в List<string> (Contains) ===");
        MeasureListStringContains(firstString, "Первый элемент");
        MeasureListStringContains(middleString, "Средний элемент");
        MeasureListStringContains(lastString, "Последний элемент");
        MeasureListStringContains(notInCollectionString, "Не в коллекции");

        Console.WriteLine("\n=== Поиск в SortedDictionary<Place, City> (ContainsKey) ===");
        MeasureSortedDictionaryPlaceKey(firstBaseCopy, "Первый элемент");
        MeasureSortedDictionaryPlaceKey(middleBaseCopy, "Средний элемент");
        MeasureSortedDictionaryPlaceKey(lastBaseCopy, "Последний элемент");
        MeasureSortedDictionaryPlaceKey(notInCollection.BasePlace, "Не в коллекции");

        Console.WriteLine("\n=== Поиск в SortedDictionary<Place, City> (ContainsValue) ===");
        MeasureSortedDictionaryPlaceValue(firstCopy, "Первый элемент");
        MeasureSortedDictionaryPlaceValue(middleCopy, "Средний элемент");
        MeasureSortedDictionaryPlaceValue(lastCopy, "Последний элемент");
        MeasureSortedDictionaryPlaceValue(notInCollection, "Не в коллекции");

        Console.WriteLine("\n=== Поиск в SortedDictionary<string, City> (ContainsKey) ===");
        MeasureSortedDictionaryStringKey(firstString, "Первый элемент");
        MeasureSortedDictionaryStringKey(middleString, "Средний элемент");
        MeasureSortedDictionaryStringKey(lastString, "Последний элемент");
        MeasureSortedDictionaryStringKey(notInCollectionString, "Не в коллекции");
    }

    static void MeasureListCityContains(City cityToFind, string description)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        bool found = testCollections.Collection1.Contains(cityToFind);
        stopwatch.Stop();
        Console.WriteLine($"{description}: {stopwatch.ElapsedTicks} тиков ({stopwatch.Elapsed.TotalSeconds:F8} секунд) (найдено: {found})");
    }

    static void MeasureListStringContains(string stringToFind, string description)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        bool found = testCollections.Collection2.Contains(stringToFind);
        stopwatch.Stop();
        Console.WriteLine($"{description}: {stopwatch.ElapsedTicks} тиков ({stopwatch.Elapsed.TotalSeconds:F8} секунд) (найдено: {found})");
    }

    static void MeasureSortedDictionaryPlaceKey(Place keyToFind, string description)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        bool found = testCollections.Collection3.ContainsKey(keyToFind);
        stopwatch.Stop();
        Console.WriteLine($"{description}: {stopwatch.ElapsedTicks} тиков ({stopwatch.Elapsed.TotalSeconds:F8} секунд) (найдено: {found})");
    }

    static void MeasureSortedDictionaryPlaceValue(City valueToFind, string description)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        bool found = testCollections.Collection3.ContainsValue(valueToFind);
        stopwatch.Stop();
        Console.WriteLine($"{description}: {stopwatch.ElapsedTicks} тиков ({stopwatch.Elapsed.TotalSeconds:F8} секунд) (найдено: {found})");
    }

    static void MeasureSortedDictionaryStringKey(string keyToFind, string description)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        bool found = testCollections.Collection4.ContainsKey(keyToFind);
        stopwatch.Stop();
        Console.WriteLine($"{description}: {stopwatch.ElapsedTicks} тиков ({stopwatch.Elapsed.TotalSeconds:F8} секунд) (найдено: {found})");
    }

    static void AddElementToTestCollections()
    {
        City newCity = testCollections.GenerateCity(testCollections.Collection1.Count);
        testCollections.AddElement(newCity);
        Console.WriteLine($"Добавлен новый элемент: {newCity}");
        Console.WriteLine($"Теперь в коллекциях: {testCollections.Collection1.Count} элементов");
    }

    static void RemoveElementFromTestCollections()
    {
        if (testCollections.Collection1.Count == 0)
        {
            Console.WriteLine("Коллекции пусты!");
            return;
        }

        Console.Write("Введите индекс элемента для удаления (0-" + (testCollections.Collection1.Count - 1) + "): ");

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < testCollections.Collection1.Count)
        {
            City cityToRemove = testCollections.Collection1[index];
            if (testCollections.RemoveElement(cityToRemove))
            {
                Console.WriteLine($"Элемент удален: {cityToRemove}");
                Console.WriteLine($"Теперь в коллекциях: {testCollections.Collection1.Count} элементов");
            }
            else
            {
                Console.WriteLine("Не удалось удалить элемент!");
            }
        }
        else
        {
            Console.WriteLine("Неверный индекс!");
        }
    }

    static void ShowTestCollectionsInfo()
    {
        Console.WriteLine("\n=== Информация о тестовых коллекциях ===");
        Console.WriteLine($"List<City>: {testCollections.Collection1.Count} элементов");
        Console.WriteLine($"List<string>: {testCollections.Collection2.Count} элементов");
        Console.WriteLine($"SortedDictionary<Place, City>: {testCollections.Collection3.Count} элементов");
        Console.WriteLine($"SortedDictionary<string, City>: {testCollections.Collection4.Count} элементов");

        if (testCollections.Collection1.Count > 0)
        {
            Console.WriteLine("\nПервые 3 элемента:");
            for (int i = 0; i < Math.Min(3, testCollections.Collection1.Count); i++)
            {
                Console.WriteLine($"[{i}] {testCollections.Collection1[i]}");
            }
        }
    }
}