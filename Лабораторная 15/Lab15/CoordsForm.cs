using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BallsGame
{
    public partial class CoordsForm : Form
    {
        private DataGridView dgv;
        private System.Windows.Forms.Timer refreshTimer;
        private MainForm mainForm;
        private object syncLock;

        public CoordsForm(MainForm mainForm, object syncLock)
        {
            this.mainForm = mainForm;
            this.syncLock = syncLock;
            InitializeComponent();
            StartRefreshTimer();
        }

        private void InitializeComponent()
        {
            this.Text = "Координаты шариков";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("X", "X");
            dgv.Columns.Add("Y", "Y");
            dgv.Columns.Add("Vx", "Vx");
            dgv.Columns.Add("Vy", "Vy");
            dgv.Columns.Add("Priority", "Приоритет потока");
            this.Controls.Add(dgv);
        }

        private void StartRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 100;
            refreshTimer.Tick += (s, e) => UpdateBallsList(mainForm.GetBallsCopy());
            refreshTimer.Start();
        }

        public void UpdateBallsList(List<Ball> balls)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<List<Ball>>(UpdateBallsList), balls);
                return;
            }

            dgv.Rows.Clear();
            lock (syncLock)
            {
                foreach (var ball in balls)
                {
                    int rowIndex = dgv.Rows.Add();
                    DataGridViewRow row = dgv.Rows[rowIndex];
                    row.Cells["Id"].Value = ball.Id;
                    row.Cells["X"].Value = ball.X.ToString("F1");
                    row.Cells["Y"].Value = ball.Y.ToString("F1");
                    row.Cells["Vx"].Value = ball.Vx.ToString("F1");
                    row.Cells["Vy"].Value = ball.Vy.ToString("F1");
                    row.Cells["Priority"].Value = ball.Thread?.Priority.ToString() ?? "Stopped";
                    row.Tag = ball;
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer?.Stop();
            base.OnFormClosed(e);
        }
    }
}