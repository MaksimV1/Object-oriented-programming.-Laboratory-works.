using System;
using System.Collections.Generic;


namespace LocationLibrary
{
    public interface IInit
    {
        void Init();
        void RandomInit();
    }

    public class Place : IInit, IComparable<Place>, ICloneable
    {
        protected string name;
        protected int population;
        protected static Random random = new Random();
        protected static string[] nameExamples = { "Бергамо", "Венеция", "Верона", "Виченца",
            "Генуя", "Десензано", "Кремона", "Ломбардия", "Максимилиана", "Милан",
            "Модена", "Парма", "Пьяченца", "Сончино", "Тревизо", "Удине" };

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
            Console.Write("Введите название места: ");
            name = Console.ReadLine();
            Console.Write("Введите население: ");
            population = int.Parse(Console.ReadLine());
        }

        public virtual void RandomInit()
        {
            name = nameExamples[random.Next(nameExamples.Length)];
            population = random.Next(10000, 1000000);
        }

        public override bool Equals(object obj)
        {
            if (obj is Place other)
                return name == other.name && population == other.population;
            return false;
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
            Console.Write("Введите федеральный округ: ");
            federalDistrict = Console.ReadLine();
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] districts = { "Центральный", "Северо-Западный", "Приволжский",
                "Уральский", "Сибирский", "Дальневосточный" };
            federalDistrict = districts[random.Next(districts.Length)];
        }

        public override bool Equals(object obj)
        {
            if (obj is Region other)
                return base.Equals(obj) && federalDistrict == other.federalDistrict;
            return false;
        }

        public override object Clone()
        {
            return new Region(name, population, federalDistrict);
        }

        public override Region ShallowCopy()
        {
            return (Region)this.MemberwiseClone();
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
            Console.Write("Введите имя основателя: ");
            founderName = Console.ReadLine();
            Console.Write("Введите год основания: ");
            foundedYear = int.Parse(Console.ReadLine());
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] founders = { "Витторио Эмануэле", "Франческо Сфорца", "Карл V", "Наполеон Бонапарт" };
            founderName = founders[random.Next(founders.Length)];
            foundedYear = random.Next(1000, 1900);
        }

        public override bool Equals(object obj)
        {
            if (obj is City other)
                return base.Equals(obj) && founderName == other.founderName && foundedYear == other.foundedYear;
            return false;
        }

        public override object Clone()
        {
            return new City(name, population, federalDistrict, founderName, foundedYear);
        }

        public override City ShallowCopy()
        {
            return (City)this.MemberwiseClone();
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
                $"Районов: {numberOfDistricts}, Площадь: {areaKm2:F2} км²");
        }

        public override void Init()
        {
            base.Init();
            Console.Write("Введите количество районов: ");
            numberOfDistricts = int.Parse(Console.ReadLine());
            Console.Write("Введите площадь в км²: ");
            areaKm2 = double.Parse(Console.ReadLine());
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

        public override object Clone()
        {
            return new Metropolis(name, population, federalDistrict, founderName, foundedYear, numberOfDistricts, areaKm2);
        }

        public override Metropolis ShallowCopy()
        {
            return (Metropolis)this.MemberwiseClone();
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
            Console.Write("Введите почтовый индекс: ");
            name = Console.ReadLine();
            Console.Write("Введите население (кол-во жилых зданий): ");
            population = int.Parse(Console.ReadLine());
            Console.Write("Введите улицу: ");
            street = Console.ReadLine();
            Console.Write("Введите номер дома: ");
            buildingNumber = int.Parse(Console.ReadLine());
            Console.Write("Введите город: ");
            city = Console.ReadLine();
        }

        public override void RandomInit()
        {
            name = (100000 + random.Next(900000)).ToString();
            population = random.Next(1, 100);
            string[] streets = { "Проспект Ленина", "Ул. Красная", "Ул. Советская",
                "Ул. Центральная", "Ул. Новая", "Ул. Фрунзе" };
            street = streets[random.Next(streets.Length)];
            buildingNumber = random.Next(1, 500);
            city = nameExamples[random.Next(nameExamples.Length)];
        }

        public override bool Equals(object obj)
        {
            if (obj is Address other)
                return name == other.name && street == other.street &&
                    buildingNumber == other.buildingNumber && city == other.city;
            return false;
        }

        public override object Clone()
        {
            return new Address(name, population, street, buildingNumber, city);
        }

        public override Address ShallowCopy()
        {
            return (Address)this.MemberwiseClone();
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
}


