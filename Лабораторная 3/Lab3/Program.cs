using System;

namespace Lab3
{
    class Program
    {
        public static double factorial = 0;
        public static double xPower = 0;

        static void Main(string[] args)
        {
            double a = 0.1;     // начало диапазона
            double b = 1.0;     // конец диапазона
            int k = 9;         // количество точек
            int n = 25;         // количество членов ряда для SN
            double epsilon = 0.0001; // точность для SE
            double step = (b - a) / k; // шаг изменения x

            Console.WriteLine("Вычисление функции");
            Console.WriteLine("X\tSN\tSE\tY");

            for (int point = 0; point <= k; point++)
            {
                double x = a + point * step;

                double SN = CalculateSN(x, n);

                double SE = CalculateSE(x, epsilon);

                double Y = CalculateY(x);

                Console.WriteLine($"{x:F2}\t{SN:F4}\t{SE:F4}\t{Y:F4}");
            }
        }

        static double CalculateSN(double x, int n)
        {
            double sum = 0;

            for (int i = 0; i <= n; i++)
            {
                double term = CalculateTerm(x, i);
                sum += term;
            }

            return sum;
        }

        static double CalculateSE(double x, double epsilon)
        {
            double sum = 0;
            double term;
            int i = 0;

            do
            {
                term = CalculateTerm(x, i);
                sum += term;
                i++;
            }
            while (Math.Abs(term) >= epsilon || i < 1000);

            return sum;
        }

        static double CalculateTerm(double x, int n)
        {
            // a_n = (cos(n * π/4) / n!) * x^n
            double cosValue = Math.Cos(n * Math.PI / 4);
            if (n == 0) factorial = 1;
            else factorial *= n;

            if (n == 0) xPower = 1;
            else xPower *= x;

            return (cosValue / factorial) * xPower;
        }

        static double CalculateY(double x)
        {
            // y = (e^(x * cos(π/4))) * (cos(x * sin(π/4)))
            double cosPi4 = Math.Cos(Math.PI / 4);
            double sinPi4 = Math.Sin(Math.PI / 4);

            double exponent = Math.Exp(x * cosPi4);
            double cosine = Math.Cos(x * sinPi4);

            return exponent * cosine;
        }
    }
}