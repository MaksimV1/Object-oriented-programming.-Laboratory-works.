using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Lab1
    {
        static void Main()
        {
            Console.WriteLine("Лабораторная работа №1\nВыполнение простой программы. Типы данных. Организация ввода и вывода данных");
            TestTask1Auto();
            TestTask2Auto();
            TestTask3Auto();

            Console.WriteLine("Программа завершена успешно.");
        }

        // ==================== ЗАДАЧА 1 ====================
        static void TestTask1Auto()
        {
            Console.WriteLine("=== ЗАДАЧА 1: АВТОМАТИЧЕСКОЕ ТЕСТИРОВАНИЕ ===");

            Console.WriteLine("\n1. Тестирование выражений с инкрементами:");
            int[,] testCases = {
            {5, 3},                         // m > 0, n > 0
            {5, -3},                        // m > 0, n < 0
            {5, 0},                         // m > 0, n = 0
            {-5, 3},                        // m < 0, n > 0
            {-5, -3},                       // m < 0, n < 0
            {-5, 0},                        // m < 0, n = 0
            {0, 3},                         // m = 0, n > 0
            {0, -3},                        // m = 0, n < 0
            {0, 0},                         // m = 0, n = 0
            {int.MaxValue, int.MaxValue},
            {int.MinValue, int.MinValue},
            {int.MaxValue, int.MinValue},
            {int.MinValue, int.MaxValue}
            };

            for (int i = 0; i < testCases.GetLength(0); i++)
            {
                Console.WriteLine($"\nТест {i + 1}: m = {testCases[i, 0]}, n = {testCases[i, 1]}");
                TestIncrementDecrement(testCases[i, 0], testCases[i, 1]);
            }

            Console.WriteLine("\n2. Тестирование выражения с корнем:");
            double[] xValues = { 
                -2,      // x < -1
                -1.0001, // близко к -1 слева
                -1,      // точно -1 (граница)
                -0.9999, // близко к -1 справа (ошибка)
                -0.5,    // -1 < x < 0 (ошибка)
                -0.0001, // близко к 0 слева (ошибка)
                0,       // точно 0 (граница)
                0.0001,  // близко к 0 справа
                1,       // типичное положительное
                100      // большое положительное 
            };

            foreach (double x in xValues)
            {
                TestExpressionWithRoot(x);
            }
        }

        // ==================== ЗАДАЧА 2 ====================
        static void TestTask2Auto()
        {
            Console.WriteLine("\n=== ЗАДАЧА 2: АВТОМАТИЧЕСКОЕ ТЕСТИРОВАНИЕ ===");

            double[,] testPoints = {
            {0, 0},          // центр
            {0, -0.5},       // внутри на оси Y
            {-0.5, 0},       // внутри на оси X
            {-0.6, -0.6},    // внутри в 3-й четверти
            {0.7, -0.7},     // внутри в 4-й четверти
            {1, 0},          // правая граница
            {-1, 0},         // левая граница
            {0, -1},         // нижняя граница
            {0.707, -0.707}, // диагональная граница
            {0, 0.5},        // над центром
            {0.5, 0.5},      // в 1-й четверти
            {-0.5, 0.5},     // во 2-й четверти
            {1.5, -0.5},     // вне круга
            {0.999, -0.001}, // близко к границе внутри
            {1.001, -0.001}  // близко к границе снаружи
            };

            for (int i = 0; i < testPoints.GetLength(0); i++)
            {
                TestPointInArea(testPoints[i, 0], testPoints[i, 1]);
            }
        }

        // ==================== ЗАДАЧА 3 ====================
        static void TestTask3Auto()
        {
            Console.WriteLine("\n=== ЗАДАЧА 3: АВТОМАТИЧЕСКОЕ ТЕСТИРОВАНИЕ ===");

            double[,] testValues = {
            {1000, 0.0001},         // заданы значения по умолчанию
            {1000000, 0.0000001},   // больший разрыв порядков
            {1, 1},                 // равные значения
            {0.0001, 1000},         // обратный порядок
            {-1000, 0.0001},        // a < 0, b > 0
            {1000, -0.0001},        // a > 0, b < 0
            {-1000, -0.0001},       // a < 0, b < 0
            {1, 0},                 // исключение
            {0, 0}                  // исключение
            };

            for (int i = 0; i < testValues.GetLength(0); i++)
            {
                Console.WriteLine($"\nТест {i + 1}:");
                CalculateExpressionDetailed(testValues[i, 0], testValues[i, 1]);
            }
        }

        // Вычисление первых 3 выражений 1 задачи
        static void TestIncrementDecrement(int m, int n)
        {
            Console.WriteLine($"\nИсходные значения: m = {m}, n = {n}");

            int m1 = m, n1 = n;
            int result1 = m1 - ++n1;
            Console.WriteLine($"1) m - ++n = {result1} (n увеличено до {n1})");

            int m2 = m, n2 = n;
            bool result2 = m2++ > --n2;
            Console.WriteLine($"2) m++ > --n = {result2} (m увеличено до {m2}, n уменьшено до {n2})");

            int m3 = m, n3 = n;
            bool result3 = m3-- < ++n3;
            Console.WriteLine($"3) m-- < ++n = {result3} (m уменьшено до {m3}, n увеличено до {n3})");
        }

        // Вычисление 4 выражения 1 задачи
        static void TestExpressionWithRoot(double x)
        {
            // Проверка допустимости вычисления корня
            double expressionUnderRoot = Math.Pow(x, 2) + x;
            bool canCompute = expressionUnderRoot >= 0;

            if (canCompute)
            {
                double result = 25 * Math.Pow(x, 5) - Math.Sqrt(expressionUnderRoot);
                Console.WriteLine($"4) Для x = {x}:");
                Console.WriteLine($"   Выражение под корнем: x^2 + x = {expressionUnderRoot}");
                Console.WriteLine($"   25x^5 - sqrt(x^2 + x) = {result:F6}");
            }
            else
            {
                Console.WriteLine($"4) Для x = {x}:");
                Console.WriteLine($"   Невозможно вычислить - отрицательное выражение под корнем: {expressionUnderRoot:F6}");
            }
        }

        // Проверка 2 задачи
        static void TestPointInArea(double x1, double y1)
        {
            double distanceSquared = Math.Pow(x1, 2) + Math.Pow(y1, 2);
            bool isInCircle = distanceSquared <= 1;
            bool isBelowOrOnXAxis = y1 <= 0;
            bool isInSemicircle = isInCircle && isBelowOrOnXAxis;

            Console.WriteLine($"\nАнализ точки ({x1}, {y1}):");
            Console.WriteLine($" Квадрат расстояния до центра: {distanceSquared:F6}");
            Console.WriteLine($" Находится внутри круга (x^2 + y^2 <= 1): {isInCircle}");
            Console.WriteLine($" Находится ниже или на оси X (y <= 0): {isBelowOrOnXAxis}");
            Console.WriteLine($" Принадлежит полуокружности: {isInSemicircle}");

            if (isInSemicircle)
            {
                Console.WriteLine("Точка принадлежит заштрихованной области");
            }
            else
            {
                Console.WriteLine("Точка НЕ принадлежит заштрихованной области");
            }
        }

        // Вычисление выражения 3 задачи
        static void CalculateExpressionDetailed(double a, double b)
        {
            Console.WriteLine($"\nВычисление для a = {a}, b = {b}:");
            // Проверка допустимости знаменателя
            bool canCompute = ((b != 0) && (3 * Math.Pow(a, 2) + Math.Pow(b, 2) != 0));

            if (canCompute)
            {
                // Вычисление с float
                float aFloat = (float)a;
                float bFloat = (float)b;

                float numeratorFloat = (float)Math.Pow(aFloat - bFloat, 3) -
                                     ((float)Math.Pow(aFloat, 3) + 3 * aFloat * (float)Math.Pow(bFloat, 2));
                float denominatorFloat = -3 * (float)Math.Pow(aFloat, 2) * bFloat - (float)Math.Pow(bFloat, 3);
                float resultFloat = numeratorFloat / denominatorFloat;

                // Вычисление с double
                double numeratorDouble = Math.Pow(a - b, 3) - (Math.Pow(a, 3) + 3 * a * Math.Pow(b, 2));
                double denominatorDouble = -3 * Math.Pow(a, 2) * b - Math.Pow(b, 3);
                double resultDouble = numeratorDouble / denominatorDouble;

                Console.WriteLine("Тип float:");
                Console.WriteLine($"  Результат: {resultFloat:F15}");
                Console.WriteLine($"  Отклонение от 1: {Math.Abs(resultFloat - 1):E5}");

                Console.WriteLine("Тип double:");
                Console.WriteLine($"  Результат: {resultDouble:F15}");
                Console.WriteLine($"  Отклонение от 1: {Math.Abs(resultDouble - 1):E5}");

                // Анализ точности
                Console.WriteLine("\nАнализ точности:");
                if (Math.Abs(resultDouble - 1) < 1e-10)
                {
                    Console.WriteLine("Double обеспечивает высокую точность");
                }
                else
                {
                    Console.WriteLine("Double имеет заметную погрешность");
                }

                if (Math.Abs(resultFloat - 1) < 1e-5)
                {
                    Console.WriteLine("Float обеспечивает приемлемую точность");
                }
                else
                {
                    Console.WriteLine("Float имеет значительную погрешность");
                }
            }
            else
            {
                Console.WriteLine("Невозможно вычислить - ноль в знаменателе");
            }
        }
    }
}
