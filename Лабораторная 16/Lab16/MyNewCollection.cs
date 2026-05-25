using LocationLibrary;
using System;
using System.Collections.Generic;

namespace Lab16
{
    public class MyNewCollection<T> : MyCollection<T> where T : IInit, ICloneable, IComparable<T>, new()
    {
        public string Name { get; set; }
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        public new TreeNode<T> Root => root; 

        protected virtual void OnCollectionCountChanged(CollectionHandlerEventArgs args) => CollectionCountChanged?.Invoke(this, args);
        protected virtual void OnCollectionReferenceChanged(CollectionHandlerEventArgs args) => CollectionReferenceChanged?.Invoke(this, args);

        public MyNewCollection() : base() => Name = "Безымянный";
        public MyNewCollection(string name) : base() => Name = name;

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
            var list = ToList();
            if (index < 0 || index >= list.Count) return false;
            T itemToRemove = list[index];
            bool removed = base.Remove(itemToRemove);
            if (removed) OnCollectionCountChanged(new CollectionHandlerEventArgs(Name, "Remove by index", itemToRemove));
            return removed;
        }

        public T this[int index]
        {
            get
            {
                var list = ToList();
                if (index < 0 || index >= list.Count) throw new IndexOutOfRangeException();
                return list[index];
            }
            set
            {
                var list = ToList();
                if (index < 0 || index >= list.Count) throw new IndexOutOfRangeException();
                list[index] = value;
                Clear();
                BuildIdealTreeFromList(list);
                OnCollectionReferenceChanged(new CollectionHandlerEventArgs(Name, "set index", value));
            }
        }

        public List<T> ToList()
        {
            var result = new List<T>();
            foreach (var item in this) result.Add(item);
            return result;
        }
    }
}
