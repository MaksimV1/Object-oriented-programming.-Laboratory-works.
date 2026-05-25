using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LocationLibrary;

namespace TreeCollection
{
    public class TreeVisualizerForm<T> : Form where T : ICloneable, IComparable<T>
    {
        private TreeNode<T> root;
        private Dictionary<TreeNode<T>, PointF> positions = new Dictionary<TreeNode<T>, PointF>();
        private float dx = 180f;
        private float dy = 120f;
        private int radius = 40;
        private float maxX = 0;
        private float maxY = 0;

        public TreeVisualizerForm(TreeNode<T> root)
        {
            this.root = root;
            this.Text = "Визуализация бинарного дерева";
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            this.ClientSize = new Size(1100, 700);
            this.AutoScroll = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (root == null) return;

            positions.Clear();
            maxX = 0;
            maxY = 0;
            int index = 0;
            ComputePositions(root, 0, ref index);

            this.AutoScrollMinSize = new Size((int)(maxX + radius * 2), (int)(maxY + radius * 2));

            Point scrollOffset = this.AutoScrollPosition;
            e.Graphics.TranslateTransform(scrollOffset.X, scrollOffset.Y);

            using (Pen pen = new Pen(Color.Black, 1.5f))
            using (Font font = new Font("Consolas", 8))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                foreach (var kvp in positions)
                {
                    TreeNode<T> node = kvp.Key;
                    PointF p = kvp.Value;

                    if (node.Left != null && positions.ContainsKey(node.Left))
                    {
                        e.Graphics.DrawLine(pen, p, positions[node.Left]);
                    }

                    if (node.Right != null && positions.ContainsKey(node.Right))
                    {
                        e.Graphics.DrawLine(pen, p, positions[node.Right]);
                    }
                }

                foreach (var kvp in positions)
                {
                    TreeNode<T> node = kvp.Key;
                    PointF p = kvp.Value;

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
            if (data is Metropolis m)
            {
                return $"Мегаполис\n" +
                       $"{m.Name}\n" +
                       $"Нас: {m.Population}\n" +
                       $"Окр: {m.FederalDistrict}\n" +
                       $"Осн: {m.FounderName}\n" +
                       $"Год: {m.FoundedYear}\n" +
                       $"Рай: {m.NumberOfDistricts}\n" +
                       $"S: {m.AreaKm2:F1}";
            }
            if (data is City c)
            {
                return $"Город\n" +
                       $"{c.Name}\n" +
                       $"Нас: {c.Population}\n" +
                       $"Окр: {c.FederalDistrict}\n" +
                       $"Осн: {c.FounderName}\n" +
                       $"Год: {c.FoundedYear}";
            }
            if (data is LocationLibrary.Region r)
            {
                return $"Область\n" +
                       $"{r.Name}\n" +
                       $"Нас: {r.Population}\n" +
                       $"Окр: {r.FederalDistrict}";
            }
            if (data is Address a)
            {
                return $"Адрес\n" +
                       $"{a.Name}\n" +
                       $"Нас: {a.Population}\n" +
                       $"{a.City}\n" +
                       $"{a.Street}\n" +
                       $"д.{a.BuildingNumber}";
            }
            if (data is Place p)
            {
                return $"Место\n" +
                       $"{p.Name}\n" +
                       $"Нас: {p.Population}";
            }
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