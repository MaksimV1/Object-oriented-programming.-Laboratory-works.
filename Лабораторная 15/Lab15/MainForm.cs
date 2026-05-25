using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BallsGame
{
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }
    }

    public partial class MainForm : Form
    {
        private BufferedPanel gamePanel;
        private Button btnShowCoords;
        private Button btnPause;
        private ComboBox cmbPriority;
        private Label lblPriority;
        private System.Windows.Forms.Timer renderTimer;
        private CoordsForm coordsForm;

        private List<Ball> balls = new List<Ball>();
        private List<Platform> platforms = new List<Platform>();
        private readonly object syncLock = new object();

        private int areaWidth = 800;
        private int areaHeight = 500;
        private Random rand = new Random();
        private const int BallRadius = 10;

        private bool _isPaused = false;
        private readonly object _pauseLock = new object();
        public bool IsPaused
        {
            get { lock (_pauseLock) return _isPaused; }
            set { lock (_pauseLock) _isPaused = value; }
        }

        public MainForm()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            this.Text = "Шарики";
            this.Size = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            gamePanel = new BufferedPanel
            {
                Location = new Point(20, 20),
                Size = new Size(areaWidth, areaHeight),
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle
            };
            gamePanel.MouseClick += GamePanel_MouseClick;
            gamePanel.Paint += GamePanel_Paint;

            btnShowCoords = new Button
            {
                Text = "Показать координаты",
                Location = new Point(areaWidth + 40, 20),
                Size = new Size(150, 30)
            };
            btnShowCoords.Click += BtnShowCoords_Click;

            btnPause = new Button
            {
                Text = "Пауза",
                Location = new Point(areaWidth + 40, 60),
                Size = new Size(150, 30),
                BackColor = Color.LightYellow
            };
            btnPause.Click += BtnPause_Click;

            lblPriority = new Label
            {
                Text = "Приоритет нового шарика:",
                Location = new Point(areaWidth + 40, 105),
                Size = new Size(150, 20)
            };

            cmbPriority = new ComboBox
            {
                Location = new Point(areaWidth + 40, 130),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPriority.Items.AddRange(new object[] { "Lowest", "BelowNormal", "Normal", "AboveNormal", "Highest" });
            cmbPriority.SelectedIndex = 2;

            this.Controls.Add(gamePanel);
            this.Controls.Add(btnShowCoords);
            this.Controls.Add(btnPause);
            this.Controls.Add(lblPriority);
            this.Controls.Add(cmbPriority);

            GenerateNonOverlappingPlatforms();
            StartRenderTimer();
        }

        private void GenerateNonOverlappingPlatforms()
        {
            int maxAttempts = 100;
            int targetCount = rand.Next(4, 7);

            for (int i = 0; i < targetCount; i++)
            {
                bool placed = false;
                for (int attempt = 0; attempt < maxAttempts && !placed; attempt++)
                {
                    int width = rand.Next(60, 100);
                    int height = rand.Next(15, 25);
                    int x = rand.Next(20, areaWidth - width - 20);
                    int y = rand.Next(areaHeight / 2, areaHeight - height - 20);
                    Rectangle newRect = new Rectangle(x, y, width, height);

                    bool overlaps = false;
                    foreach (var plat in platforms)
                    {
                        if (newRect.IntersectsWith(plat.Bounds))
                        {
                            overlaps = true;
                            break;
                        }
                    }
                    if (!overlaps)
                    {
                        platforms.Add(new Platform(newRect));
                        placed = true;
                    }
                }
                if (!placed)
                {
                    int width = 50, height = 15;
                    int x = rand.Next(20, areaWidth - width - 20);
                    int y = rand.Next(areaHeight / 2, areaHeight - height - 20);
                    platforms.Add(new Platform(new Rectangle(x, y, width, height)));
                }
            }
        }

        private void StartRenderTimer()
        {
            renderTimer = new System.Windows.Forms.Timer();
            renderTimer.Interval = 30;
            renderTimer.Tick += (s, e) => gamePanel.Invalidate();
            renderTimer.Start();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            lock (syncLock)
            {
                foreach (var plat in platforms)
                    plat.Draw(g);
                foreach (var ball in balls)
                    ball.Draw(g);
            }
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsPaused) return;
            if (e.X < BallRadius || e.X > areaWidth - BallRadius || e.Y < BallRadius || e.Y > areaHeight - BallRadius)
                return;

            float vx = (float)(rand.NextDouble() * 6 - 3);
            float vy = (float)(rand.NextDouble() * 6 - 3);
            Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            ThreadPriority priority = GetSelectedPriority();

            Ball newBall = null;
            lock (syncLock)
            {
                newBall = new Ball(e.X, e.Y, vx, vy, randomColor,
                                   syncLock, platforms, areaWidth, areaHeight,
                                   OnBallRemoved, () => IsPaused);
                balls.Add(newBall);
            }
            newBall.Start(priority);
            coordsForm?.UpdateBallsList(balls);
        }

        private ThreadPriority GetSelectedPriority()
        {
            switch (cmbPriority.SelectedItem.ToString())
            {
                case "Lowest": return ThreadPriority.Lowest;
                case "BelowNormal": return ThreadPriority.BelowNormal;
                case "Normal": return ThreadPriority.Normal;
                case "AboveNormal": return ThreadPriority.AboveNormal;
                case "Highest": return ThreadPriority.Highest;
                default: return ThreadPriority.Normal;
            }
        }

        private void OnBallRemoved(Ball ball)
        {
            lock (syncLock)
            {
                if (balls.Contains(ball))
                    balls.Remove(ball);
            }
            ball.Stop();
            this.Invoke((MethodInvoker)(() => coordsForm?.UpdateBallsList(balls)));
        }

        private void BtnShowCoords_Click(object sender, EventArgs e)
        {
            if (coordsForm == null || coordsForm.IsDisposed)
            {
                coordsForm = new CoordsForm(this, syncLock);
                coordsForm.FormClosed += (s, args) => coordsForm = null;
                coordsForm.Show();
            }
            else
            {
                coordsForm.BringToFront();
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            IsPaused = !IsPaused;
            btnPause.Text = IsPaused ? "Старт" : "Пауза";
            btnPause.BackColor = IsPaused ? Color.LightCoral : Color.LightYellow;
        }

        public List<Ball> GetBallsCopy()
        {
            lock (syncLock)
            {
                return new List<Ball>(balls);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            lock (syncLock)
            {
                foreach (var ball in balls)
                    ball.Stop();
                balls.Clear();
            }
            Thread.Sleep(100);
            base.OnFormClosing(e);
        }
    }
}