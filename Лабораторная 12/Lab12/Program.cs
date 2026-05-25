using LocationLibrary;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;

namespace TreeCollection
{
    public static class LocationRandomFactory
    {
        private static Random rnd = new Random();

        public static Place GetRandomPlace()
        {
            int type = rnd.Next(5);
            switch (type)
            {
                case 0:
                    {
                        Place p = new Place();
                        p.RandomInit();
                        return p;
                    }
                case 1:
                    {
                        LocationLibrary.Region r = new LocationLibrary.Region();
                        r.RandomInit();
                        return r;
                    }
                case 2:
                    {
                        City c = new City();
                        c.RandomInit();
                        return c;
                    }
                case 3:
                    {
                        Metropolis m = new Metropolis();
                        m.RandomInit();
                        return m;
                    }
                default:
                    {
                        Address a = new Address();
                        a.RandomInit();
                        return a;
                    }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MyCollection<Place> tree = null;
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("===========================================");
                Console.WriteLine(" МЕНЮ ");
                Console.WriteLine("1. Сформировать сбалансированное дерево");
                Console.WriteLine("2. Вывести текущее дерево");
                Console.WriteLine("3. Найти среднее арифметическое элементов дерева (по населению)");
                Console.WriteLine("4. Преобразовать дерево в дерево поиска");
                Console.WriteLine("5. Демонстрация foreach (обход дерева)");
                Console.WriteLine("6. Демонстрация глубокого и поверхностного копирования");
                Console.WriteLine("7. Добавить один или несколько элементов в дерево");
                Console.WriteLine("8. Удалить элемент из дерева");
                Console.WriteLine("9. Удалить дерево из памяти");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        {
                            Console.Write("Введите количество элементов дерева: ");
                            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
                            {
                                Console.WriteLine("Некорректное число.");
                                break;
                            }

                            List<Place> list = new List<Place>();
                            for (int i = 0; i < n; i++)
                            {
                                list.Add(LocationRandomFactory.GetRandomPlace());
                            }

                            tree = new MyCollection<Place>(n);
                            tree.BuildIdealTreeFromList(list);
                            Console.WriteLine("Красно-черное дерево сформировано.");
                            break;
                        }

                    case "2":
                        {
                            if (tree == null || tree.Count == 0)
                            {
                                Console.WriteLine("Дерево не создано или пустое.");
                            }
                            else
                            {
                                Console.WriteLine("Откроется окно визуализации дерева.");
                                tree.ShowInForm();
                            }
                            break;
                        }

                    case "3":
                        {
                            if (tree == null || tree.Count == 0)
                            {
                                Console.WriteLine("Дерево не создано или пустое.");
                            }
                            else
                            {
                                double avg = tree.AveragePopulation();
                                Console.WriteLine($"Среднее арифметическое населения элементов дерева: {avg:F2}");
                            }
                            break;
                        }

                    case "4":
                        {
                            if (tree == null || tree.Count == 0)
                            {
                                Console.WriteLine("Дерево не создано или пустое.");
                            }
                            else
                            {
                                tree.TransformToSearchTree();
                                Console.WriteLine("Дерево преобразовано в дерево поиска (по населению).");
                            }
                            break;
                        }

                    case "5":
                        {
                            if (tree == null || tree.Count == 0)
                            {
                                Console.WriteLine("Дерево не создано или пустое.");
                            }
                            else
                            {
                                Console.WriteLine("Перебор элементов дерева с помощью foreach (in-order):");
                                int i = 0;
                                foreach (Place p in tree)
                                {
                                    Console.Write($"{i}. ");
                                    p.Show();
                                    i++;
                                }
                            }
                            break;
                        }

                    case "6":
                        {
                            if (tree == null || tree.Count == 0)
                            {
                                Console.WriteLine("Дерево не создано или пустое.");
                            }
                            else
                            {
                                Console.WriteLine("Создание глубокого клона дерева...");
                                MyCollection<Place> deep = tree.DeepClone();
                                Console.WriteLine("Создание поверхностной копии дерева...");
                                MyCollection<Place> shallow = tree.ShallowCopy();

                                Console.WriteLine("Оригинал (первые несколько элементов, foreach):");
                                int k = 0;
                                foreach (Place p in tree)
                                {
                                    p.Show();
                                    if (++k >= 3) break;
                                }

                                Console.WriteLine("\nГлубокий клон (первые несколько элементов, foreach):");
                                k = 0;
                                foreach (Place p in deep)
                                {
                                    p.Show();
                                    if (++k >= 3) break;
                                }

                                Console.WriteLine("\nПоверхностная копия (первые несколько элементов, foreach):");
                                k = 0;
                                foreach (Place p in shallow)
                                {
                                    p.Show();
                                    if (++k >= 3) break;
                                }
                            }
                            break;
                        }

                    case "7":
                        {
                            if (tree == null)
                            {
                                Console.WriteLine("Коллекция не была создана. Она будет создана автоматически.");
                                tree = new MyCollection<Place>();
                            }

                            Console.Write("Введите количество добавляемых элементов: ");
                            if (!int.TryParse(Console.ReadLine(), out int m) || m <= 0)
                            {
                                Console.WriteLine("Некорректное число.");
                                break;
                            }

                            List<Place> addList = new List<Place>();
                            for (int i = 0; i < m; i++)
                            {
                                addList.Add(LocationRandomFactory.GetRandomPlace());
                            }

                            tree.AddRange(addList);
                            Console.WriteLine($"Добавлено {m} случайных элементов в дерево.");
                            break;
                        }

                    case "8":
                        {
                            Console.WriteLine("Выберете способ удаления");
                            Console.WriteLine("1. По индексу");
                            Console.WriteLine("2. По значению (по населению)");
                            Console.Write("Ваш выбор: ");
                            input = Console.ReadLine();
                            while (input != "1" && input != "2")
                            {
                                Console.WriteLine("Неизвестная команда. Введите снова");
                                Console.Write("Ваш выбор: ");
                                input = Console.ReadLine();
                            }
                            bool removed = false;
                            int criteria = -1;
                            int i = 0;
                            if (input == "1")
                            {
                                Console.Write("Введите индекс элемента: ");
                                criteria = int.Parse(Console.ReadLine());
                            }
                            else
                            {
                                Console.Write("Введите население элемента: ");
                                criteria = int.Parse(Console.ReadLine());
                            }

                            foreach (Place p in tree)
                            {
                                if (input == "1")
                                {
                                    if (criteria == i)
                                    {
                                        removed = tree.Remove(p);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (criteria == p.Population)
                                    {
                                        removed = tree.Remove(p);
                                        break;
                                    }
                                }
                                i++;
                            }
                            if (removed)
                            {
                                Console.WriteLine("Элемент успешно удален");
                            }
                            else
                            {
                                Console.WriteLine("Не удалось удалить элемент");
                            }
                            break;
                        }

                    case "9":
                        {
                            if (tree != null)
                            {
                                tree.DeleteFromMemory();
                                tree = null;
                                Console.WriteLine("Дерево удалено из памяти.");
                            }
                            else
                            {
                                Console.WriteLine("Дерево не создано.");
                            }
                            break;
                        }

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
