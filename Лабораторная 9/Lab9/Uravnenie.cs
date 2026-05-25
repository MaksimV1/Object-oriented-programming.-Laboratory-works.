using System;

//public class Uravnenie
//{
//    private static int count = 0;
//    private double a;
//    private double b;
//    private double c;

//    public double A
//    {
//        get { return a; }
//        set
//        {
//            if (value == 0)
//                throw new ArgumentException("Коэффициент a не может быть равен 0 для квадратного уравнения");
//            a = value;
//        }
//    }

//    public double B
//    {
//        get { return b; }
//        set { b = value; }
//    }

//    public double C
//    {
//        get { return c; }
//        set { c = value; }
//    }

//    public static int Count => count;

//    public Uravnenie()
//    {
//        A = 1;
//        B = 0;
//        C = 0;
//        count++;
//    }

//    public Uravnenie(double a, double b, double c)
//    {
//        A = a;
//        B = b;
//        C = c;
//        count++;
//    }

//    private double GetDiscriminant()
//    {
//        return B * B - 4 * A * C;
//    }

//    public UravnenieResultType CalculateRoots(out double root1, out double root2)
//    {
//        root1 = 0;
//        root2 = 0;

//        double discriminant = GetDiscriminant();

//        if (discriminant > 0)
//        {
//            root1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
//            root2 = (-B - Math.Sqrt(discriminant)) / (2 * A);
//            return UravnenieResultType.TwoSolutions;
//        }
//        else if (discriminant == 0)
//        {
//            root1 = -B / (2 * A);
//            return UravnenieResultType.OneSolution;
//        }
//        else
//        {
//            return UravnenieResultType.NoRealSolutions;
//        }
//    }

//    public double GetOneRoot()
//    {
//        double root1, root2;
//        var resultType = CalculateRoots(out root1, out root2);

//        if (resultType == UravnenieResultType.OneSolution || resultType == UravnenieResultType.TwoSolutions)
//        {
//            return root1;
//        }
//        return 0;
//    }

//    public bool HasRoots()
//    {
//        double root1, root2;
//        var resultType = CalculateRoots(out root1, out root2);
//        return resultType == UravnenieResultType.OneSolution || resultType == UravnenieResultType.TwoSolutions;
//    }

//    public static Uravnenie operator ++(Uravnenie eq)
//    {
//        eq.A += 1;
//        eq.B += 1;
//        eq.C += 1;
//        return eq;
//    }

//    public static Uravnenie operator --(Uravnenie eq)
//    {
//        eq.A -= 1;
//        eq.B -= 1;
//        eq.C -= 1;
//        return eq;
//    }

//    public static implicit operator double(Uravnenie eq)
//    {
//        return eq.GetOneRoot();
//    }

//    public static explicit operator bool(Uravnenie eq)
//    {
//        return eq.HasRoots();
//    }

//    public static bool operator ==(Uravnenie t1, Uravnenie t2)
//    {
//        if (ReferenceEquals(t1, t2)) return true;
//        if (t1 is null || t2 is null) return false;
//        return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C;
//    }

//    public static bool operator !=(Uravnenie t1, Uravnenie t2)
//    {
//        return !(t1 == t2);
//    }
//}

//public enum UravnenieResultType
//{
//    NoRealSolutions, 
//    OneSolution, 
//    TwoSolutions 
//}