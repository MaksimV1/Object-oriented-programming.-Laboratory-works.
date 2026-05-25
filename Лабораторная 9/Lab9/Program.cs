using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Практическая работа №9 «Классы и объекты» ===");

        try
        {
            DemonstrateCollection();

            DemonstratePart1();

            DemonstratePart2();

            DemonstrateErrorHandling();

            DemonstrateEquationSearch();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}");
        }

        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void DemonstrateCollection()
    {
        Console.WriteLine("\n=== Коллекция уравнений ===");

        Console.WriteLine("\n1. Конструктор без параметров (5 уравнений по умолчанию):");
        UravnenieCollection collection1 = new UravnenieCollection();
        collection1.PrintAll();

        Console.WriteLine("\n2. Конструктор с параметрами (случайные уравнения, размер = 4):");
        UravnenieCollection collection2 = new UravnenieCollection(4);
        collection2.PrintAll();

        Console.WriteLine("\n3. Конструктор с параметрами (массив из 3 заданных уравнений):");
        Uravnenie[] equations = new Uravnenie[]
        {
            new Uravnenie(1, -3, 2),
            new Uravnenie(2, 5, -3),
            new Uravnenie(1, -2, 1)
        };
        UravnenieCollection collection3 = new UravnenieCollection(equations);
        collection3.PrintAll();

        Console.WriteLine("\n4. Демонстрация работы индексатора:");
        Console.WriteLine($"Элемент с индексом 0:");
        PrintEquation(collection3[0]);
        Console.WriteLine($"Элемент с индексом 1:");
        PrintEquation(collection3[1]);

        try
        {
            Console.WriteLine($"\nПопытка получить элемент с индексом 10:");
            var test = collection3[10];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine($"\nОбщее количество созданных объектов: {Uravnenie.Count}");
    }

    static void DemonstratePart1()
    {
        Console.WriteLine("\n=== Часть 1: Базовый функционал класса ===");

        List<Uravnenie> equations = new List<Uravnenie>
        {
            new Uravnenie(1, -3, 2),
            new Uravnenie(1, -2, 1),
            new Uravnenie(1, 2, 3),
            new Uravnenie(2, 5, -3),
            new Uravnenie(4, 4, 1)
        };

        Console.WriteLine("\nСозданные уравнения:");
        for (int i = 0; i < equations.Count; i++)
        {
            Console.Write($"Уравнение {i + 1}: ");
            PrintEquation(equations[i]);
        }

        Console.WriteLine("\n--- Метод класса для вычисления корней ---");
        foreach (var eq in equations)
        {
            Console.Write("Уравнение: ");
            PrintEquation(eq);
            Console.Write("Результат: ");
            PrintEquationResult(eq);
        }

        Console.WriteLine($"\nВсего создано объектов: {Uravnenie.Count}");
    }

    static void DemonstratePart2()
    {
        Console.WriteLine("\n=== Часть 2: Перегруженные операции ===");

        Console.WriteLine("\n--- Унарные операции ---");

        Uravnenie eqForIncrement = new Uravnenie(2, -3, 1);
        Console.Write("Исходное уравнение: ");
        PrintEquation(eqForIncrement);
        eqForIncrement++;
        Console.Write("После операции ++: ");
        PrintEquation(eqForIncrement);

        Uravnenie eqForDecrement = new Uravnenie(3, -2, 1);
        Console.Write("Исходное уравнение: ");
        PrintEquation(eqForDecrement);
        eqForDecrement--;
        Console.Write("После операции --: ");
        PrintEquation(eqForDecrement);

        Console.WriteLine("\n--- Приведение типов ---");
        Uravnenie eq1 = new Uravnenie(1, -3, 2);
        double root = eq1;
        Console.WriteLine($"Неявное приведение к double: {root:F2}");

        bool hasRoots = (bool)eq1;
        Console.WriteLine($"Явное приведение к bool: {hasRoots}");

        Uravnenie eqNoRoots = new Uravnenie(1, 2, 3);
        double rootNoRoots = eqNoRoots;
        Console.WriteLine($"Уравнение без корней, приведение к double: {rootNoRoots:F2}");

        bool hasRootsNoRoots = (bool)eqNoRoots;
        Console.WriteLine($"Уравнение без корней, приведение к bool: {hasRootsNoRoots}");

        Console.WriteLine("\n--- Бинарные операции ---");
        Uravnenie eq2 = new Uravnenie(1, -3, 2);
        Uravnenie eq3 = new Uravnenie(2, -3, 1);

        Console.Write("eq1: "); PrintEquation(eq1);
        Console.Write("eq2: "); PrintEquation(eq2);
        Console.Write("eq3: "); PrintEquation(eq3);

        Console.WriteLine($"eq1 == eq2: {eq1 == eq2}");
        Console.WriteLine($"eq1 == eq3: {eq1 == eq3}");
        Console.WriteLine($"eq1 != eq3: {eq1 != eq3}");

        Console.WriteLine($"\nОбщее количество созданных объектов: {Uravnenie.Count}");
    }

    static void DemonstrateErrorHandling()
    {
        Console.WriteLine("\n=== Демонстрация обработки ошибок ===");

        try
        {
            Console.WriteLine("Попытка создать уравнение с a=0:");
            Uravnenie invalidEq = new Uravnenie(0, 2, 3);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            Console.WriteLine("\nПопытка изменить коэффициент a на 0:");
            Uravnenie eq = new Uravnenie(1, 2, 3);
            eq.A = 0;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            Console.WriteLine("\nПопытка использовать оператор -- когда a=1:");
            Uravnenie eq = new Uravnenie(1, 2, 3);
            eq--;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void DemonstrateEquationSearch()
    {
        Console.WriteLine("\n=== Поиск уравнения с самым большим по абсолютному значению корнем ===");

        Uravnenie[] equationsArray = new Uravnenie[]
        {
            new Uravnenie(1, -5, 6),
            new Uravnenie(1, 0, -9), 
            new Uravnenie(1, 2, 1),
            new Uravnenie(2, -7, 3),
            new Uravnenie(1, 2, 5)
        };

        UravnenieCollection collection = new UravnenieCollection(equationsArray);
        collection.PrintAll();

        Uravnenie maxRootEquation = FindEquationWithLargestRoot(collection);

        if (maxRootEquation != null)
        {
            Console.WriteLine($"\nУравнение с самым большим по абсолютному значению корнем:");
            PrintEquation(maxRootEquation);

            double root1, root2;
            var resultType = maxRootEquation.CalculateRoots(out root1, out root2);

            if (resultType == UravnenieResultType.OneSolution)
            {
                Console.WriteLine($"Корень: x = {root1:F2}, |x| = {Math.Abs(root1):F2}");
            }
            else if (resultType == UravnenieResultType.TwoSolutions)
            {
                double maxAbsRoot = Math.Max(Math.Abs(root1), Math.Abs(root2));
                Console.WriteLine($"Корни: x1 = {root1:F2}, x2 = {root2:F2}");
                Console.WriteLine($"Наибольший по абсолютному значению корень: |x| = {maxAbsRoot:F2}");
            }
        }
        else
        {
            Console.WriteLine("\nВ коллекции нет уравнений с действительными корнями.");
        }
    }

    static Uravnenie FindEquationWithLargestRoot(UravnenieCollection collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        Uravnenie result = null;
        double maxAbsValue = double.MinValue;

        for (int i = 0; i < collection.Count; i++)
        {
            try
            {
                double root1, root2;
                var resultType = collection[i].CalculateRoots(out root1, out root2);

                if (resultType == UravnenieResultType.OneSolution)
                {
                    double absValue = Math.Abs(root1);
                    if (absValue > maxAbsValue)
                    {
                        maxAbsValue = absValue;
                        result = collection[i];
                    }
                }
                else if (resultType == UravnenieResultType.TwoSolutions)
                {
                    double maxRootAbs = Math.Max(Math.Abs(root1), Math.Abs(root2));
                    if (maxRootAbs > maxAbsValue)
                    {
                        maxAbsValue = maxRootAbs;
                        result = collection[i];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке уравнения с индексом {i}: {ex.Message}");
            }
        }

        return result;
    }

    static void PrintEquation(Uravnenie eq)
    {
        Console.WriteLine($"{eq.A}x^2 + {eq.B}x + {eq.C} = 0");
    }

    static void PrintEquationResult(Uravnenie eq)
    {
        double root1, root2;
        var resultType = eq.CalculateRoots(out root1, out root2);

        switch (resultType)
        {
            case UravnenieResultType.NoRealSolutions:
                Console.WriteLine("Действительных корней нет");
                break;
            case UravnenieResultType.OneSolution:
                Console.WriteLine($"Один корень: x = {root1:F2}");
                break;
            case UravnenieResultType.TwoSolutions:
                Console.WriteLine($"Два различных корня: x1 = {root1:F2}, x2 = {root2:F2}");
                break;
        }
    }
}