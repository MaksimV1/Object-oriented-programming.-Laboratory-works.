using System.Drawing;

namespace BallsGame
{
    public class Platform
    {
        public Rectangle Bounds { get; set; }
        public float Elasticity { get; set; } = 0.8f;

        public Platform(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color.FromArgb(100, 150, 100, 50)))
            {
                g.FillRectangle(brush, Bounds);
            }
            g.DrawRectangle(Pens.Brown, Bounds);
        }
    }
}