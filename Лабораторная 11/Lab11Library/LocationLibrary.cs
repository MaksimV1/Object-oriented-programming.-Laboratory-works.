using System;
using System.Collections.Generic;

namespace LocationLibrary
{
    public interface IInit
    {
        void Init();
        void RandomInit();
    }

    public class Place : IInit, IComparable, IComparable<Place>, ICloneable
    {
        protected string name;
        protected int population;
        protected static Random random = new Random();

        public static string[] NameExamples = { "Бергамо", "Венеция", "Верона", "Виченца", "Генуя", 
            "Десензано", "Кремона", "Ломбардия", "Милан", "Модена", "Парма", "Пьяченца", "Сончино", 
            "Тревизо", "Удине", "Турин", "Флоренция", "Рим", "Неаполь", "Болонья", "Бари", "Палермо",
            "Катания", "Мессина", "Перуджа", "Анкона", "Пескара", "Л'Акуила", "Кампания", "Лацио",
            "Тоскана", "Лигурия", "Пьемонт", "Венето", "Эмилия-Романья", "Париж", "Лондон", "Берлин", 
            "Мадрид", "Барселона", "Амстердам", "Вена", "Прага", "Варшава", "Будапешт", "Бухарест", "София" };

        public string Name
        {
            get { return name; }
            set
            {
                if (value == "" || value == null)
                    throw new ArgumentException("Имя не может быть пустым");
                name = value;
            }
        }

        public int Population
        {
            get { return population; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Население не может быть отрицательным");
                population = value;
            }
        }

        public Place()
        {
            name = "Безымянное место";
            population = 0;
        }

        public Place(string name, int population)
        {
            Name = name;
            Population = population;
        }

        public Place(Place other)
        {
            name = other.name;
            population = other.population;
        }

        public virtual void Show()
        {
            Console.WriteLine($"Место: {name}, Население: {population}");
        }

        public virtual void Init()
        {
            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите название места: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Название не может быть пустым. Попробуйте снова.");
                        continue;
                    }
                    name = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите население (целое положительное число): ");
                    if (!int.TryParse(Console.ReadLine(), out int pop))
                    {
                        Console.WriteLine("Ошибка: Введите корректное целое число для населения.");
                        continue;
                    }

                    if (pop < 0)
                    {
                        Console.WriteLine("Ошибка: Население не может быть отрицательным.");
                        continue;
                    }

                    population = pop;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное целое число.");
                }
            }
        }

        public virtual void RandomInit()
        {
            name = NameExamples[random.Next(NameExamples.Length)];
            population = random.Next(10000, 1000000);
        }

        public override bool Equals(object obj)
        {
            if (obj is Place other)
                return name == other.name && population == other.population;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, population);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Place other)
                return CompareTo(other);
            throw new ArgumentException("Object is not a Place");
        }

        public virtual int CompareTo(Place other)
        {
            if (other == null) return 1;
            return population.CompareTo(other.population);
        }

        public virtual object Clone()
        {
            return new Place(name, population);
        }

        public virtual Place ShallowCopy()
        {
            return (Place)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{name} ({population} чел.)";
        }
    }

    public class Region : Place
    {
        protected string federalDistrict;

        public string FederalDistrict
        {
            get { return federalDistrict; }
            set
            {
                if (value == "" || value == null)
                    throw new ArgumentException("Федеральный округ не может быть пустым");
                federalDistrict = value;
            }
        }

        public Region() : base()
        {
            federalDistrict = "Неизвестный округ";
        }

        public Region(string name, int population, string federalDistrict)
            : base(name, population)
        {
            FederalDistrict = federalDistrict;
        }

        public Region(Region other) : base(other)
        {
            federalDistrict = other.federalDistrict;
        }

        public override void Show()
        {
            Console.WriteLine($"Область: {name}, Население: {population}, Округ: {federalDistrict}");
        }

        public override void Init()
        {
            base.Init();

            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите федеральный округ: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Федеральный округ не может быть пустым. Попробуйте снова.");
                        continue;
                    }
                    federalDistrict = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] districts = { "Центральный", "Северо-Западный", "Южный", "Северо-Кавказский",
                "Приволжский", "Уральский", "Сибирский", "Дальневосточный", "Прибалтийский", "Кавказский", 
                "Прикаспийский", "Приарктический", "Тихоокеанский", "Атлантический", "Средиземноморский",
                "Ленинградская область", "Московская область", "Киевская Русь","Великое княжество Московское", 
                "Сибирское ханство", "Казанское ханство", "Новгородская республика", "Псковская республика"};
            federalDistrict = districts[random.Next(districts.Length)];
        }

        public override bool Equals(object obj)
        {
            if (obj is Region other)
                return base.Equals(obj) && federalDistrict == other.federalDistrict;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), federalDistrict);
        }

        public override object Clone()
        {
            return new Region(name, population, federalDistrict);
        }

        public override Region ShallowCopy()
        {
            return (Region)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Округ: {federalDistrict}";
        }
    }

    public class City : Region
    {
        protected string founderName;
        protected int foundedYear;

        public string FounderName
        {
            get { return founderName; }
            set
            {
                if (value == "" || value == null)
                    throw new ArgumentException("Имя основателя не может быть пустым");
                founderName = value;
            }
        }

        public int FoundedYear
        {
            get { return foundedYear; }
            set
            {
                if (value < 0 || value > DateTime.Now.Year)
                    throw new ArgumentException("Некорректный год основания");
                foundedYear = value;
            }
        }

        public Place BasePlace
        {
            get
            {
                return new Place(name, population);
            }
        }

        public City() : base()
        {
            founderName = "Неизвестный";
            foundedYear = DateTime.Now.Year;
        }

        public City(string name, int population, string federalDistrict, string founderName, int foundedYear)
            : base(name, population, federalDistrict)
        {
            FounderName = founderName;
            FoundedYear = foundedYear;
        }

        public City(City other) : base(other)
        {
            founderName = other.founderName;
            foundedYear = other.foundedYear;
        }

        public override void Show()
        {
            Console.WriteLine($"Город: {name}, Население: {population}, Округ: {federalDistrict}, " +
                $"Основатель: {founderName}, Год основания: {foundedYear}");
        }

        public override void Init()
        {
            base.Init();

            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите имя основателя: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Имя основателя не может быть пустым. Попробуйте снова.");
                        continue;
                    }
                    founderName = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write($"Введите год основания (0-{DateTime.Now.Year}): ");
                    if (!int.TryParse(Console.ReadLine(), out int year))
                    {
                        Console.WriteLine("Ошибка: Введите корректное целое число для года основания.");
                        continue;
                    }

                    if (year < 0 || year > DateTime.Now.Year)
                    {
                        Console.WriteLine($"Ошибка: Год основания должен быть от 0 до {DateTime.Now.Year}.");
                        continue;
                    }

                    foundedYear = year;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное целое число.");
                }
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] founders = { "Витторио Эмануэле", "Франческо Сфорца", "Карл V", "Наполеон Бонапарт",
                "Петр I", "Екатерина II", "Иван Грозный", "Александр Невский", "Дмитрий Донской", 
                "Ярослав Мудрый", "Владимир Мономах", "Юрий Долгорукий", "Андрей Боголюбский", "Иван Калита", 
                "Юлий Цезарь", "Октавиан Август", "Траян", "Константин Великий", "Марк Аврелий", "Адриан", 
                "Нерон", "Калигула", "Клавдий", "Карл Великий", "Вильгельм Завоеватель", "Ричард Львиное Сердце",
                "Людовик XIV", "Фридрих Великий", "Мария Терезия", "Елизавета I", "Генрих VIII", "Филипп II" };
            founderName = founders[random.Next(founders.Length)];
            foundedYear = random.Next(1000, 1900);
        }

        public override bool Equals(object obj)
        {
            if (obj is City other)
                return base.Equals(obj) && founderName == other.founderName && foundedYear == other.foundedYear;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), founderName, foundedYear);
        }

        public override object Clone()
        {
            return new City(name, population, federalDistrict, founderName, foundedYear);
        }

        public override City ShallowCopy()
        {
            return (City)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Основатель: {founderName}, Год основания: {foundedYear}";
        }
    }

    public class Metropolis : City
    {
        protected int numberOfDistricts;
        protected double areaKm2;

        public int NumberOfDistricts
        {
            get { return numberOfDistricts; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Количество районов должно быть больше 0");
                numberOfDistricts = value;
            }
        }

        public double AreaKm2
        {
            get { return areaKm2; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Площадь должна быть больше 0");
                areaKm2 = value;
            }
        }

        public Metropolis() : base()
        {
            numberOfDistricts = 1;
            areaKm2 = 0.0;
        }

        public Metropolis(string name, int population, string federalDistrict, string founderName,
            int foundedYear, int numberOfDistricts, double areaKm2)
            : base(name, population, federalDistrict, founderName, foundedYear)
        {
            NumberOfDistricts = numberOfDistricts;
            AreaKm2 = areaKm2;
        }

        public Metropolis(Metropolis other) : base(other)
        {
            numberOfDistricts = other.numberOfDistricts;
            areaKm2 = other.areaKm2;
        }

        public override void Show()
        {
            Console.WriteLine($"Мегаполис: {name}, Население: {population}, Округ: {federalDistrict}, " +
                $"Основатель: {founderName}, Год основания: {foundedYear}, " +
                $"Районов: {numberOfDistricts}, Площадь: {areaKm2:F2} км^2");
        }

        public override void Init()
        {
            base.Init();

            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите количество районов (целое число больше 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int districts))
                    {
                        Console.WriteLine("Ошибка: Введите корректное целое число для количества районов.");
                        continue;
                    }

                    if (districts < 1)
                    {
                        Console.WriteLine("Ошибка: Количество районов должно быть больше 0.");
                        continue;
                    }

                    numberOfDistricts = districts;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное целое число.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите площадь в км^2 (число больше 0): ");
                    if (!double.TryParse(Console.ReadLine(), out double area))
                    {
                        Console.WriteLine("Ошибка: Введите корректное число для площади.");
                        continue;
                    }

                    if (area <= 0)
                    {
                        Console.WriteLine("Ошибка: Площадь должна быть больше 0.");
                        continue;
                    }

                    areaKm2 = area;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное число.");
                }
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            numberOfDistricts = random.Next(5, 25);
            areaKm2 = random.Next(100, 10000) + random.NextDouble();
        }

        public override bool Equals(object obj)
        {
            if (obj is Metropolis other)
                return base.Equals(obj) && numberOfDistricts == other.numberOfDistricts &&
                    Math.Abs(areaKm2 - other.areaKm2) < 0.01;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), numberOfDistricts, areaKm2);
        }

        public override object Clone()
        {
            return new Metropolis(name, population, federalDistrict, founderName, foundedYear, numberOfDistricts, areaKm2);
        }

        public override Metropolis ShallowCopy()
        {
            return (Metropolis)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Районов: {numberOfDistricts}, Площадь: {areaKm2:F2} км^2";
        }
    }

    public class Address : Place
    {
        protected string street;
        protected int buildingNumber;
        protected string city;

        public string Street
        {
            get { return street; }
            set
            {
                if (value == "" || value == null)
                    throw new ArgumentException("Улица не может быть пустой");
                street = value;
            }
        }

        public int BuildingNumber
        {
            get { return buildingNumber; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Номер дома должен быть больше 0");
                buildingNumber = value;
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value == "" || value == null)
                    throw new ArgumentException("Город не может быть пустым");
                city = value;
            }
        }

        public Address() : base()
        {
            street = "Неизвестная улица";
            buildingNumber = 1;
            city = "Неизвестный город";
        }

        public Address(string name, int population, string street, int buildingNumber, string city)
            : base(name, population)
        {
            Street = street;
            BuildingNumber = buildingNumber;
            City = city;
        }

        public Address(Address other) : base(other)
        {
            street = other.street;
            buildingNumber = other.buildingNumber;
            city = other.city;
        }

        public override void Show()
        {
            Console.WriteLine($"Адрес: {name}, Улица: {street}, Дом: {buildingNumber}, Город: {city}");
        }

        public override void Init()
        {
            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите почтовый индекс (6 цифр): ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Почтовый индекс не может быть пустым.");
                        continue;
                    }

                    if (input.Length != 6 || !int.TryParse(input, out _))
                    {
                        Console.WriteLine("Ошибка: Почтовый индекс должен состоять из 6 цифр.");
                        continue;
                    }

                    name = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите население (кол-во жилых зданий, целое положительное число): ");
                    if (!int.TryParse(Console.ReadLine(), out int pop))
                    {
                        Console.WriteLine("Ошибка: Введите корректное целое число для населения.");
                        continue;
                    }

                    if (pop < 0)
                    {
                        Console.WriteLine("Ошибка: Население не может быть отрицательным.");
                        continue;
                    }

                    population = pop;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное целое число.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите улицу: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Улица не может быть пустой.");
                        continue;
                    }
                    street = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите номер дома (целое число больше 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int number))
                    {
                        Console.WriteLine("Ошибка: Введите корректное целое число для номера дома.");
                        continue;
                    }

                    if (number < 1)
                    {
                        Console.WriteLine("Ошибка: Номер дома должен быть больше 0.");
                        continue;
                    }

                    buildingNumber = number;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: Введите корректное целое число.");
                }
            }

            flag = true;
            while (flag)
            {
                try
                {
                    Console.Write("Введите город: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка: Город не может быть пустым.");
                        continue;
                    }
                    city = input;
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
                }
            }
        }

        public override void RandomInit()
        {
            name = (100000 + random.Next(900000)).ToString();
            population = random.Next(1, 100);
            string[] streets = { "Проспект Ленина", "Улица Красная", "Улица Советская", 
                "Улица Центральная", "Улица Новая", "Улица Фрунзе", "Улица Кирова", "Улица Горького",
                "Невский проспект", "Тверская улица", "Арбат", "Кутузовский проспект",
                "Ленинградский проспект", "Московский проспект", "Киевская улица",
                "Улица Пушкина", "Улица Гоголя", "Улица Чехова", "Улица Толстого",
                "Улица Достоевского", "Улица Маяковского", "Улица Есенина",
                "Улица Гагарина", "Улица Королева", "Улица Циолковского",
                "Улица Речная", "Улица Лесная", "Улица Полевая", "Улица Горная",
                "Улица Озерная", "Улица Морская", "Улица Садовая", "Улица Парковая" };
            street = streets[random.Next(streets.Length)];
            buildingNumber = random.Next(1, 500);
            city = NameExamples[random.Next(NameExamples.Length)];
        }

        public override bool Equals(object obj)
        {
            if (obj is Address other)
                return name == other.name && street == other.street &&
                    buildingNumber == other.buildingNumber && city == other.city;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), street, buildingNumber, city);
        }

        public override object Clone()
        {
            return new Address(name, population, street, buildingNumber, city);
        }

        public override Address ShallowCopy()
        {
            return (Address)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Улица: {street}, Дом: {buildingNumber}, Город: {city}";
        }
    }

    public class LocationManager
    {
        public static void ShowAllVirtual(Place[] locations)
        {
            Console.WriteLine("\n=== Вывод через виртуальные функции ===");
            foreach (Place loc in locations)
            {
                loc.Show();
            }
        }

        public static void ShowAllNonVirtual(Place[] locations)
        {
            Console.WriteLine("\n=== Вывод через невиртуальные функции (тип Place) ===");
            foreach (Place loc in locations)
            {
                Console.Write($"Место: {loc.Name}, Население: {loc.Population}");
                Console.WriteLine();
            }
        }
    }

    public class TestCollections
    {
        public List<City> Collection1 { get; private set; }
        public List<string> Collection2 { get; private set; }
        public SortedDictionary<Place, City> Collection3 { get; private set; }
        public SortedDictionary<string, City> Collection4 { get; private set; }

        public TestCollections(int count)
        {
            Collection1 = new List<City>();
            Collection2 = new List<string>();
            Collection3 = new SortedDictionary<Place, City>();
            Collection4 = new SortedDictionary<string, City>();

            for (int i = 0; i < count; i++)
            {
                City city = GenerateCity(i);

                Collection1.Add(city);
                Collection2.Add(city.ToString());
                Collection3.Add(city.BasePlace, city);
                Collection4.Add(city.ToString(), city);
            }
        }

        public City GenerateCity(int index)
        {
            Random random = new Random();
            string[] founders = { "Витторио Эмануэле", "Франческо Сфорца", "Карл V", "Наполеон Бонапарт",
                "Петр I", "Екатерина II", "Иван Грозный", "Александр Невский", "Дмитрий Донской",
                "Ярослав Мудрый", "Владимир Мономах", "Юрий Долгорукий", "Андрей Боголюбский", "Иван Калита",
                "Юлий Цезарь", "Октавиан Август", "Траян", "Константин Великий", "Марк Аврелий", "Адриан",
                "Нерон", "Калигула", "Клавдий", "Карл Великий", "Вильгельм Завоеватель", "Ричард Львиное Сердце",
                "Людовик XIV", "Фридрих Великий", "Мария Терезия", "Елизавета I", "Генрих VIII", "Филипп II" };

            string[] districts = { "Центральный", "Северо-Западный", "Южный", "Северо-Кавказский",
                "Приволжский", "Уральский", "Сибирский", "Дальневосточный", "Прибалтийский", "Кавказский",
                "Прикаспийский", "Приарктический", "Тихоокеанский", "Атлантический", "Средиземноморский",
                "Ленинградская область", "Московская область", "Киевская Русь","Великое княжество Московское",
                "Сибирское ханство", "Казанское ханство", "Новгородская республика", "Псковская республика"};

            string[] nameExamples = Place.NameExamples;

            string name = $"{nameExamples[index % nameExamples.Length]}_{index}";
            int population = 10000 + index * 1000;
            string district = districts[index % districts.Length];
            string founder = founders[index % founders.Length];
            int foundedYear = 1000 + index % 900;

            return new City(name, population, district, founder, foundedYear);
        }

        public City GenerateCityNotInCollection()
        {
            return GenerateCity(Collection1.Count + 1000);
        }

        public void AddElement(City city)
        {
            Collection1.Add(city);
            Collection2.Add(city.ToString());
            Collection3.Add(city.BasePlace, city);
            Collection4.Add(city.ToString(), city);
        }

        public bool RemoveElement(City city)
        {
            bool removed = Collection1.Remove(city);
            if (removed)
            {
                Collection2.Remove(city.ToString());
                Collection3.Remove(city.BasePlace);
                Collection4.Remove(city.ToString());
            }
            return removed;
        }
    }
}