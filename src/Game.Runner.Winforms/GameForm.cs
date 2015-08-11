using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Game.Controllers;
using Game.Core;
using Newtonsoft.Json;

namespace Game.Running.Winforms
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public NotSoGreatController Controller { get; set; }
        public GameState CurrentGameState { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var problem = Problem.FromFile(openFileDialog1.FileName);
                Controller = new NotSoGreatController(problem);
                CurrentGameState = Controller.GameState;
                boardControl1.GameState = Controller.GameState;
                Text = openFileDialog1.FileName;

                RefreshGameState();
                Controller.OnMove += state =>
                {
                    if (DrawMovesBox.Checked)
                    {
                        RefreshGameState();
                        Thread.Sleep(75);                        
                    }
                    Application.DoEvents();
                };

                Controller.OnLock += state =>
                {
                    RefreshGameState();
                    Thread.Sleep(100);
                };

                Controller.OnGameOver += state =>
                {
                    RefreshGameState();                    
                };
            }
        }

        public void RefreshGameState()
        {
            UnitsRemainingLabel.Text = CurrentGameState.UnitsLeft.ToString();
            ScoreLabel.Text = CurrentGameState.Score.ToString();
            Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentGameState.SpawnNextUnit();
            RefreshGameState();
        }

        private void ButtonW_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.W);
            RefreshGameState();
        }

        private void ButtonE_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.E);
            RefreshGameState();
        }

        private void ButtonSw_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.SW);
            RefreshGameState();
        }

        private void ButtonSe_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.SE);
            RefreshGameState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var outputs = Controller.Solve();

            File.WriteAllText("out.json", JsonConvert.SerializeObject(outputs));
            SolutionBox.Text = "";
            foreach (var o in outputs)
            {
                SolutionBox.Text += o.tag + "\n";
            }
            //SolutionBox.Text = CurrentGameState.Moves.ToSolutionString();
        }

        private void CWButton_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.CW);
            RefreshGameState();
        }

        private void CCWButton_Click(object sender, EventArgs e)
        {
            CurrentGameState.ExecuteMove(Direction.CCW);
            RefreshGameState();
        }
    }
}