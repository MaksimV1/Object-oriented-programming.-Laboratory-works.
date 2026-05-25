using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LocationLibrary;

namespace Lab16
{
    public class TreeVisualizerForm<T> : Form where T : ICloneable, IComparable<T>
    {
        private TreeNode<T> root;
        private Dictionary<TreeNode<T>, PointF> positions = new Dictionary<TreeNode<T>, PointF>();
        private float dx = 180f;
        private float dy = 120f;
        private int radius = 40;
        private float maxX = 0, maxY = 0;

        public TreeVisualizerForm(TreeNode<T> root)
        {
            this.root = root;
            Text = "Визуализация бинарного дерева";
            BackColor = Color.White;
            DoubleBuffered = true;
            ClientSize = new Size(1100, 700);
            AutoScroll = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (root == null) return;
            positions.Clear();
            maxX = maxY = 0;
            int index = 0;
            ComputePositions(root, 0, ref index);
            AutoScrollMinSize = new Size((int)(maxX + radius * 2), (int)(maxY + radius * 2));
            Point scrollOffset = AutoScrollPosition;
            e.Graphics.TranslateTransform(scrollOffset.X, scrollOffset.Y);
            using (Pen pen = new Pen(Color.Black, 1.5f))
            using (Font font = new Font("Consolas", 8))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                foreach (var kvp in positions)
                {
                    var node = kvp.Key;
                    var p = kvp.Value;
                    if (node.Left != null && positions.ContainsKey(node.Left)) e.Graphics.DrawLine(pen, p, positions[node.Left]);
                    if (node.Right != null && positions.ContainsKey(node.Right)) e.Graphics.DrawLine(pen, p, positions[node.Right]);
                }
                foreach (var kvp in positions)
                {
                    var node = kvp.Key;
                    var p = kvp.Value;
                    RectangleF rect = new RectangleF(p.X - radius, p.Y - radius, radius * 2, radius * 2);
                    e.Graphics.FillEllipse(Brushes.LightBlue, rect);
                    e.Graphics.DrawEllipse(pen, rect);
                    string text = GetNodeText(node.Data);
                    e.Graphics.DrawString(text, font, Brushes.Black, rect, sf);
                }
            }
        }

        private string GetNodeText(T data)
        {
            if (data is Metropolis m) return $"Мегаполис\n{m.Name}\nНас: {m.Population}\n{m.FederalDistrict}\n{m.FounderName}\n{m.FoundedYear}\nРай: {m.NumberOfDistricts}\nS: {m.AreaKm2:F1}";
            if (data is City c) return $"Город\n{c.Name}\nНас: {c.Population}\n{c.FederalDistrict}\n{c.FounderName}\n{c.FoundedYear}";
            if (data is LocationLibrary.Region r) return $"Область\n{r.Name}\nНас: {r.Population}\n{r.FederalDistrict}";
            if (data is Address a) return $"Адрес\n{a.Name}\n{a.City}\n{a.Street}\n{a.BuildingNumber}";
            if (data is Place p) return $"Место\n{p.Name}\n{p.Population}";
            return "?";
        }

        private void ComputePositions(TreeNode<T> node, int depth, ref int index)
        {
            if (node == null) return;
            ComputePositions(node.Left, depth + 1, ref index);
            float x = (index + 1) * dx;
            float y = (depth + 1) * dy;
            positions[node] = new PointF(x, y);
            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
            index++;
            ComputePositions(node.Right, depth + 1, ref index);
        }
    }
}