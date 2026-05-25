using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LocationLibrary
{
    public interface IInit
    {
        void Init();
        void RandomInit();
    }

    [Serializable]
    [XmlInclude(typeof(Region))]
    [XmlInclude(typeof(City))]
    [XmlInclude(typeof(Metropolis))]
    [XmlInclude(typeof(Address))]
    [JsonDerivedType(typeof(Region), nameof(Region))]
    [JsonDerivedType(typeof(City), nameof(City))]
    [JsonDerivedType(typeof(Metropolis), nameof(Metropolis))]
    [JsonDerivedType(typeof(Address), nameof(Address))]
    public class Place : IInit, IComparable<Place>, ICloneable
    {
        protected string name;
        protected int population;
        protected static Random random = new Random();
        protected static string[] nameExamples = { "Бергамо", "Венеция", "Верона", "Виченца",
            "Генуя", "Десензано", "Кремона", "Ломбардия", "Максимилиана", "Милан",
            "Модена", "Парма", "Пьяченца", "Сончино", "Тревизо", "Удине" };

        public string Name { get => name; set => name = string.IsNullOrEmpty(value) ? throw new ArgumentException("Имя не может быть пустым") : value; }
        public int Population { get => population; set => population = value < 0 ? throw new ArgumentException("Население не может быть отрицательным") : value; }

        public Place() { name = "Безымянное место"; population = 0; }
        public Place(string name, int population) { Name = name; Population = population; }
        public Place(Place other) { name = other.name; population = other.population; }

        public virtual void Show() => Console.WriteLine($"Место: {name}, Население: {population}");
        public virtual void Init()
        {
            Console.Write("Введите название: "); name = Console.ReadLine();
            Console.Write("Введите население: "); population = int.Parse(Console.ReadLine());
        }
        public virtual void RandomInit()
        {
            name = nameExamples[random.Next(nameExamples.Length)];
            population = random.Next(10000, 1000000);
        }
        public override bool Equals(object obj) => obj is Place other && name == other.name && population == other.population;
        public override int GetHashCode() => HashCode.Combine(name, population);
        public virtual int CompareTo(Place other) => other == null ? 1 : population.CompareTo(other.population);
        public virtual object Clone() => new Place(name, population);
        public virtual Place ShallowCopy() => (Place)MemberwiseClone();
        public override string ToString() => $"Place: {name}, Pop: {population}";
    }

    [Serializable]
    public class Region : Place
    {
        protected string federalDistrict;
        public string FederalDistrict { get => federalDistrict; set => federalDistrict = string.IsNullOrEmpty(value) ? throw new ArgumentException("Округ не может быть пустым") : value; }
        public Region() : base() { federalDistrict = "Неизвестный округ"; }
        public Region(string name, int population, string federalDistrict) : base(name, population) => FederalDistrict = federalDistrict;
        public Region(Region other) : base(other) => federalDistrict = other.federalDistrict;
        public override void Show() => Console.WriteLine($"Область: {name}, Население: {population}, Округ: {federalDistrict}");
        public override void Init() { base.Init(); Console.Write("Федеральный округ: "); federalDistrict = Console.ReadLine(); }
        public override void RandomInit()
        {
            base.RandomInit();
            string[] districts = { "Центральный", "Северо-Западный", "Приволжский", "Уральский", "Сибирский", "Дальневосточный" };
            federalDistrict = districts[random.Next(districts.Length)];
        }
        public override bool Equals(object obj) => obj is Region other && base.Equals(obj) && federalDistrict == other.federalDistrict;
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), federalDistrict);
        public override object Clone() => new Region(name, population, federalDistrict);
        public override Region ShallowCopy() => (Region)MemberwiseClone();
        public override string ToString() => $"Region: {name}, Pop: {population}, District: {federalDistrict}";
    }

    [Serializable]
    public class City : Region
    {
        protected string founderName;
        protected int foundedYear;
        public string FounderName { get => founderName; set => founderName = string.IsNullOrEmpty(value) ? throw new ArgumentException("Имя основателя не может быть пустым") : value; }
        public int FoundedYear { get => foundedYear; set => foundedYear = (value < 0 || value > DateTime.Now.Year) ? throw new ArgumentException("Некорректный год") : value; }
        public City() : base() { founderName = "Неизвестный"; foundedYear = DateTime.Now.Year; }
        public City(string name, int population, string federalDistrict, string founderName, int foundedYear) : base(name, population, federalDistrict)
        { FounderName = founderName; FoundedYear = foundedYear; }
        public City(City other) : base(other) { founderName = other.founderName; foundedYear = other.foundedYear; }
        public override void Show() => Console.WriteLine($"Город: {name}, Население: {population}, Округ: {federalDistrict}, Основатель: {founderName}, Год: {foundedYear}");
        public override void Init() { base.Init(); Console.Write("Основатель: "); founderName = Console.ReadLine(); Console.Write("Год основания: "); foundedYear = int.Parse(Console.ReadLine()); }
        public override void RandomInit()
        {
            base.RandomInit();
            string[] founders = { "Витторио Эмануэле", "Франческо Сфорца", "Карл V", "Наполеон Бонапарт" };
            founderName = founders[random.Next(founders.Length)];
            foundedYear = random.Next(1000, 1900);
        }
        public override bool Equals(object obj) => obj is City other && base.Equals(obj) && founderName == other.founderName && foundedYear == other.foundedYear;
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), founderName, foundedYear);
        public override object Clone() => new City(name, population, federalDistrict, founderName, foundedYear);
        public override City ShallowCopy() => (City)MemberwiseClone();
        public override string ToString() => $"City: {name}, Pop: {population}, District: {federalDistrict}, Founder: {founderName}, Year: {foundedYear}";
    }

    [Serializable]
    public class Metropolis : City
    {
        protected int numberOfDistricts;
        protected double areaKm2;
        public int NumberOfDistricts { get => numberOfDistricts; set => numberOfDistricts = value < 1 ? throw new ArgumentException("Районов >0") : value; }
        public double AreaKm2 { get => areaKm2; set => areaKm2 = value <= 0 ? throw new ArgumentException("Площадь >0") : value; }
        public Metropolis() : base() { numberOfDistricts = 1; areaKm2 = 0; }
        public Metropolis(string name, int population, string federalDistrict, string founderName, int foundedYear, int numberOfDistricts, double areaKm2)
            : base(name, population, federalDistrict, founderName, foundedYear)
        { NumberOfDistricts = numberOfDistricts; AreaKm2 = areaKm2; }
        public Metropolis(Metropolis other) : base(other) { numberOfDistricts = other.numberOfDistricts; areaKm2 = other.areaKm2; }
        public override void Show() => Console.WriteLine($"Мегаполис: {name}, Население: {population}, Округ: {federalDistrict}, Основатель: {founderName}, Год: {foundedYear}, Районов: {numberOfDistricts}, Площадь: {areaKm2:F2} км²");
        public override void Init() { base.Init(); Console.Write("Районов: "); numberOfDistricts = int.Parse(Console.ReadLine()); Console.Write("Площадь км²: "); areaKm2 = double.Parse(Console.ReadLine()); }
        public override void RandomInit()
        {
            base.RandomInit();
            numberOfDistricts = random.Next(5, 25);
            areaKm2 = random.Next(100, 10000) + random.NextDouble();
        }
        public override bool Equals(object obj) => obj is Metropolis other && base.Equals(obj) && numberOfDistricts == other.numberOfDistricts && Math.Abs(areaKm2 - other.areaKm2) < 0.01;
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), numberOfDistricts, areaKm2);
        public override object Clone() => new Metropolis(name, population, federalDistrict, founderName, foundedYear, numberOfDistricts, areaKm2);
        public override Metropolis ShallowCopy() => (Metropolis)MemberwiseClone();
        public override string ToString() => $"Metropolis: {name}, Pop: {population}, Districts: {numberOfDistricts}, Area: {areaKm2:F2}";
    }

    [Serializable]
    public class Address : Place
    {
        protected string street;
        protected int buildingNumber;
        protected string city;
        public string Street { get => street; set => street = string.IsNullOrEmpty(value) ? throw new ArgumentException("Улица не может быть пустой") : value; }
        public int BuildingNumber { get => buildingNumber; set => buildingNumber = value < 1 ? throw new ArgumentException("Номер дома >0") : value; }
        public string City { get => city; set => city = string.IsNullOrEmpty(value) ? throw new ArgumentException("Город не может быть пустым") : value; }
        public Address() : base() { street = "Неизвестная улица"; buildingNumber = 1; city = "Неизвестный город"; }
        public Address(string name, int population, string street, int buildingNumber, string city) : base(name, population)
        { Street = street; BuildingNumber = buildingNumber; City = city; }
        public Address(Address other) : base(other) { street = other.street; buildingNumber = other.buildingNumber; city = other.city; }
        public override void Show() => Console.WriteLine($"Адрес: {name}, Улица: {street}, Дом: {buildingNumber}, Город: {city}");
        public override void Init()
        {
            Console.Write("Индекс: "); name = Console.ReadLine();
            Console.Write("Кол-во жилых зданий: "); population = int.Parse(Console.ReadLine());
            Console.Write("Улица: "); street = Console.ReadLine();
            Console.Write("Номер дома: "); buildingNumber = int.Parse(Console.ReadLine());
            Console.Write("Город: "); city = Console.ReadLine();
        }
        public override void RandomInit()
        {
            name = (100000 + random.Next(900000)).ToString();
            population = random.Next(1, 100);
            string[] streets = { "Проспект Ленина", "Ул. Красная", "Ул. Советская", "Ул. Центральная", "Ул. Новая", "Ул. Фрунзе" };
            street = streets[random.Next(streets.Length)];
            buildingNumber = random.Next(1, 500);
            city = nameExamples[random.Next(nameExamples.Length)];
        }
        public override bool Equals(object obj) => obj is Address other && name == other.name && street == other.street && buildingNumber == other.buildingNumber && city == other.city;
        public override int GetHashCode() => HashCode.Combine(name, street, buildingNumber, city);
        public override object Clone() => new Address(name, population, street, buildingNumber, city);
        public override Address ShallowCopy() => (Address)MemberwiseClone();
        public override string ToString() => $"Address: {name}, Street: {street}, Building: {buildingNumber}, City: {city}";
    }
}