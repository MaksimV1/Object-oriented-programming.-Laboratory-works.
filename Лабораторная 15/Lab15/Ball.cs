using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace BallsGame
{
    public class Ball
    {
        private const int UpdateIntervalMs = 20;
        private const float Gravity = 0.35f;

        public float X { get; set; }
        public float Y { get; set; }
        public float Vx { get; set; }
        public float Vy { get; set; }
        public int Radius { get; set; } = 10;
        public Color Color { get; set; }

        public Thread Thread { get; private set; }
        public bool IsRunning { get; private set; } = true;
        public int Id { get; private set; }

        private static int _nextId = 1;
        private readonly object _syncLock;
        private readonly List<Platform> _platforms;
        private readonly Action<Ball> _onBallRemoved;
        private readonly int _areaWidth;
        private readonly int _areaHeight;
        private readonly Func<bool> _isPausedFunc;

        public Ball(float x, float y, float vx, float vy, Color color,
                    object syncLock, List<Platform> platforms, int areaWidth, int areaHeight,
                    Action<Ball> onBallRemoved, Func<bool> isPausedFunc)
        {
            Id = _nextId++;
            X = x;
            Y = y;
            Vx = vx;
            Vy = vy;
            Color = color;
            _syncLock = syncLock;
            _platforms = platforms;
            _areaWidth = areaWidth;
            _areaHeight = areaHeight;
            _onBallRemoved = onBallRemoved;
            _isPausedFunc = isPausedFunc;
        }

        public void Start(ThreadPriority priority)
        {
            Thread = new Thread(Run);
            Thread.Priority = priority;
            Thread.IsBackground = true;
            Thread.Start();
        }

        public void Stop() => IsRunning = false;

        private void Run()
        {
            while (IsRunning)
            {
                while (IsRunning && _isPausedFunc())
                {
                    Thread.Sleep(50);
                }
                if (!IsRunning) break;

                float newX, newY, newVx, newVy;
                bool fallen = false;

                lock (_syncLock)
                {
                    newVx = Vx;
                    newVy = Vy;
                    newVy += Gravity;
                    newX = X + newVx;
                    newY = Y + newVy;

                    if ((newX - Radius < 0) || (newX + Radius > _areaWidth) || (newY - Radius < 0) || (newY + Radius >= _areaHeight))
                    {
                        fallen = true;
                    }
                    else
                    {
                        float testX = newX;
                        float testY = newY;
                        HandlePlatformCollisions(ref testX, ref testY, ref newVx, ref newVy);
                        newX = testX;
                        newY = testY;

                        X = newX;
                        Y = newY;
                        Vx = newVx;
                        Vy = newVy;
                    }
                }

                if (fallen)
                {
                    _onBallRemoved(this);
                    return;
                }

                Thread.Sleep(UpdateIntervalMs);
            }
        }

        private void HandlePlatformCollisions(ref float x, ref float y, ref float vx, ref float vy)
        {
            foreach (var platform in _platforms)
            {
                Rectangle plat = platform.Bounds;
                if (x + Radius > plat.Left && x - Radius < plat.Right &&
                    y + Radius > plat.Top && y - Radius < plat.Bottom)
                {
                    float overlapLeft = (x + Radius) - plat.Left;
                    float overlapRight = plat.Right - (x - Radius);
                    float overlapTop = (y + Radius) - plat.Top;
                    float overlapBottom = plat.Bottom - (y - Radius);

                    float minOverlap = Math.Min(Math.Min(overlapLeft, overlapRight),
                                                Math.Min(overlapTop, overlapBottom));

                    if (minOverlap == overlapLeft || minOverlap == overlapRight)
                    {
                        vx = -vx * platform.Elasticity;
                        if (minOverlap == overlapLeft)
                            x = plat.Left - Radius;
                        else
                            x = plat.Right + Radius;
                    }
                    else
                    {
                        vy = -vy * platform.Elasticity;
                        if (minOverlap == overlapTop)
                            y = plat.Top - Radius;
                        else
                            y = plat.Bottom + Radius;
                    }
                    break;
                }
            }
        }

        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }
            g.DrawEllipse(Pens.Black, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }
}