using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using LocationLibrary;

namespace Lab16
{
    public class TreeNode<T>
    {
        public T Data;
        public TreeNode<T> Left;
        public TreeNode<T> Right;
        public TreeNode(T data) { Data = data; Left = Right = null; }
    }

    public class TreeEnumerator<T> : IEnumerator<T> where T : ICloneable, IComparable<T>
    {
        private readonly TreeNode<T> root;
        private readonly Stack<TreeNode<T>> stack;
        private TreeNode<T> currentNode;
        public TreeEnumerator(TreeNode<T> root) { this.root = root; stack = new Stack<TreeNode<T>>(); Reset(); }
        public T Current { get { if (currentNode == null) throw new InvalidOperationException(); return currentNode.Data; } }
        object IEnumerator.Current => throw new Exception();
        public void Dispose() => GC.SuppressFinalize(this);
        public bool MoveNext()
        {
            while (stack.Count > 0)
            {
                TreeNode<T> node = stack.Pop();
                if (node.Right != null) PushLeft(node.Right);
                currentNode = node;
                return true;
            }
            currentNode = null;
            return false;
        }
        private void PushLeft(TreeNode<T> node) { while (node != null) { stack.Push(node); node = node.Left; } }
        public void Reset() { stack.Clear(); currentNode = null; if (root != null) PushLeft(root); }
    }

    public class MyCollection<T> : ICollection<T> where T : ICloneable, IComparable<T>
    {
        internal TreeNode<T> root;
        private int count;
        private int capacity;

        public MyCollection() { root = null; count = 0; capacity = 0; }
        public MyCollection(int capacity) { root = null; count = 0; this.capacity = capacity; }
        public MyCollection(MyCollection<T> c)
        {
            if (c == null) { root = null; count = 0; capacity = 0; return; }
            capacity = c.capacity;
            count = c.count;
            root = CloneNodeDeep(c.root);
        }
        private TreeNode<T> CloneNodeDeep(TreeNode<T> node)
        {
            if (node == null) return null;
            T clonedData = (T)node.Data.Clone();
            TreeNode<T> newNode = new TreeNode<T>(clonedData);
            newNode.Left = CloneNodeDeep(node.Left);
            newNode.Right = CloneNodeDeep(node.Right);
            return newNode;
        }
        private TreeNode<T> CloneNodeShallow(TreeNode<T> node)
        {
            if (node == null) return null;
            TreeNode<T> newNode = node;
            newNode.Left = CloneNodeShallow(node.Left);
            newNode.Right = CloneNodeShallow(node.Right);
            return newNode;
        }
        private TreeNode<T> BuildIdealBalanced(List<T> items, int start, int end)
        {
            if (start > end) return null;
            int mid = (start + end) / 2;
            TreeNode<T> node = new TreeNode<T>(items[mid]);
            node.Left = BuildIdealBalanced(items, start, mid - 1);
            node.Right = BuildIdealBalanced(items, mid + 1, end);
            return node;
        }
        private void InsertToSearchTree(ref TreeNode<T> node, T data)
        {
            if (node == null) { node = new TreeNode<T>(data); return; }
            if (data.CompareTo(node.Data) < 0) InsertToSearchTree(ref node.Left, data);
            else InsertToSearchTree(ref node.Right, data);
        }
        private void InOrderCollect(TreeNode<T> node, List<T> list)
        {
            if (node == null) return;
            InOrderCollect(node.Left, list);
            list.Add(node.Data);
            InOrderCollect(node.Right, list);
        }
        private bool ContainsNode(TreeNode<T> node, T item)
        {
            if (node == null) return false;
            if (Equals(node.Data, item)) return true;
            return ContainsNode(node.Left, item) || ContainsNode(node.Right, item);
        }
        private int CountNodes(TreeNode<T> node) => node == null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);
        private void SumPopulation(TreeNode<T> node, Func<T, double> selector, ref double sum, ref int cnt)
        {
            if (node == null) return;
            SumPopulation(node.Left, selector, ref sum, ref cnt);
            sum += selector(node.Data);
            cnt++;
            SumPopulation(node.Right, selector, ref sum, ref cnt);
        }
        private TreeNode<T> RemoveNode(TreeNode<T> node, T value, ref bool deleted)
        {
            if (node == null) return null;
            int cmp = value.CompareTo(node.Data);
            if (cmp < 0) node.Left = RemoveNode(node.Left, value, ref deleted);
            else if (cmp > 0) node.Right = RemoveNode(node.Right, value, ref deleted);
            else
            {
                deleted = true;
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;
                TreeNode<T> min = node.Right;
                while (min.Left != null) min = min.Left;
                node.Data = min.Data;
                node.Right = RemoveNode(node.Right, min.Data, ref deleted);
            }
            return node;
        }

        public void BuildIdealTreeFromList(List<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            count = items.Count;
            root = BuildIdealBalanced(items, 0, items.Count - 1);
        }
        public double Average(Func<T, double> selector)
        {
            if (root == null) return 0.0;
            double sum = 0; int cnt = 0;
            SumPopulation(root, selector, ref sum, ref cnt);
            return cnt == 0 ? 0.0 : sum / cnt;
        }
        public double AveragePopulation() => Average(item => (item is Place p) ? p.Population : 0.0);
        public void TransformToSearchTree()
        {
            if (root == null) return;
            List<T> items = new List<T>();
            InOrderCollect(root, items);
            root = null;
            count = 0;
            foreach (T item in items) InsertToSearchTree(ref root, item);
            count = CountNodes(root);
        }
        public void DeleteFromMemory() { root = null; count = 0; }
        public MyCollection<T> DeepClone() { MyCollection<T> clone = new MyCollection<T>(capacity); clone.root = CloneNodeDeep(root); clone.count = count; return clone; }
        public MyCollection<T> ShallowCopy() { MyCollection<T> copy = new MyCollection<T>(capacity); copy.root = CloneNodeShallow(root); copy.count = count; return copy; }
        public void AddRange(IEnumerable<T> items) { foreach (T item in items) Add(item); }
        public void ShowInForm()
        {
            if (root == null) { Console.WriteLine("Дерево пустое"); return; }
            using (TreeVisualizerForm<T> form = new TreeVisualizerForm<T>(root)) Application.Run(form);
        }
        public int Count => count;
        public bool IsReadOnly => false;
        public void Add(T item) { InsertToSearchTree(ref root, item); count = CountNodes(root); }
        public void Clear() => DeleteFromMemory();
        public bool Contains(T item) => ContainsNode(root, item);
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Недостаточная длина массива.");
            List<T> items = new List<T>();
            InOrderCollect(root, items);
            for (int i = 0; i < items.Count; i++) array[arrayIndex + i] = items[i];
        }
        public bool Remove(T item) { bool deleted = false; root = RemoveNode(root, item, ref deleted); if (deleted) count = CountNodes(root); return deleted; }
        public IEnumerator<T> GetEnumerator() => new TreeEnumerator<T>(root);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}