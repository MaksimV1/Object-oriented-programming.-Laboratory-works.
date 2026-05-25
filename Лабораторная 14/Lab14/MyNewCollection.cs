using LocationLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TreeCollection;

namespace Lab14
{
    public class MyNewCollection<T> : MyCollection<T>
        where T : IInit, ICloneable, IComparable<T>, new()
    {
        public string Name { get; set; }

        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        protected virtual void OnCollectionCountChanged(CollectionHandlerEventArgs args)
        {
            if (CollectionCountChanged != null)
                CollectionCountChanged(this, args);
        }

        protected virtual void OnCollectionReferenceChanged(CollectionHandlerEventArgs args)
        {
            if (CollectionReferenceChanged != null)
                CollectionReferenceChanged(this, args);
        }

        public MyNewCollection() : base()
        {
            Name = "Безымянный";
        }

        public MyNewCollection(string name) : base()
        {
            Name = name;
        }

        public void AddRandom(int count = 3)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = new T();
                obj.RandomInit();
                Add(obj);
                OnCollectionCountChanged(new CollectionHandlerEventArgs(Name, "AddRandom", obj));
            }
        }

        public void Add(T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (T item in items)
            {
                Add(item);
                OnCollectionCountChanged(new CollectionHandlerEventArgs(Name, "Add range", item));
            }
        }

        public bool Remove(int index)
        {
            List<T> list = ToList();
            if (index < 0 || index >= list.Count)
                return false;

            T itemToRemove = list[index];
            bool removed = base.Remove(itemToRemove);
            if (removed)
            {
                OnCollectionCountChanged(new CollectionHandlerEventArgs(Name, "Remove by index", itemToRemove));
            }
            return removed;
        }

        public T this[int index]
        {
            get
            {
                List<T> list = ToList();
                if (index < 0 || index >= list.Count)
                    throw new IndexOutOfRangeException("Индекс вне границ коллекции");
                return list[index];
            }
            set
            {
                List<T> list = ToList();
                if (index < 0 || index >= list.Count)
                    throw new IndexOutOfRangeException("Индекс вне границ коллекции");

                list[index] = value;
                Clear();
                BuildIdealTreeFromList(list);

                OnCollectionReferenceChanged(new CollectionHandlerEventArgs(Name, "set index", value));
            }
        }

        private List<T> ToList()
        {
            List<T> list = new List<T>();
            foreach (T item in this)
            {
                list.Add(item);
            }
            return list;
        }
    }
}