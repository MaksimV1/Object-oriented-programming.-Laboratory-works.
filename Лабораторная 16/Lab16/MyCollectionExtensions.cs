using LocationLibrary;
using System;
using System.Collections.Generic;

namespace Lab16
{
    public static class MyCollectionExtensions
    {
        public static IEnumerable<T> Filter<T>(this MyNewCollection<T> collection, Func<T, bool> predicate)
            where T : IInit, ICloneable, IComparable<T>, new()
        {
            foreach (var item in collection) if (predicate(item)) yield return item;
        }

        public static double AveragePopulation<T>(this MyNewCollection<T> collection, Func<T, double> selector)
            where T : IInit, ICloneable, IComparable<T>, new()
        {
            double sum = 0; int count = 0;
            foreach (var item in collection) { sum += selector(item); count++; }
            return count == 0 ? 0 : sum / count;
        }

        public static List<T> OrderByAlphabet<T, TKey>(this MyNewCollection<T> collection, Func<T, TKey> keySelector)
            where T : IInit, ICloneable, IComparable<T>, new()
            where TKey : IComparable<TKey>
        {
            List<T> list = new List<T>();
            foreach (var item in collection) list.Add(item);
            list.Sort((a, b) => keySelector(a).CompareTo(keySelector(b)));
            return list;
        }
    }
}