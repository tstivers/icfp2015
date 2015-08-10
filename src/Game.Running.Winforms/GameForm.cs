using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public GameState CurrentGameState { get; set; }
        public LockSpaceSearcher LockSpaceSearcher { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var problem = Problem.FromFile(openFileDialog1.FileName);
                CurrentGameState = new GameState(problem, null);
                boardControl1.GameState = CurrentGameState;
                Text = openFileDialog1.FileName;

                RefreshGameState();
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
            var outputs = new List<Output>();
            for (var i = 0; i < CurrentGameState.Problem.SourceSeeds.Length; i++)
            {
                CurrentGameState.Reset(i);
                try
                {
                    while (true)
                    {
                        LockSpaceSearcher = new LockSpaceSearcher();
                        LockSpaceSearcher.GameState = CurrentGameState;
                        LockSpaceSearcher.GenerateLockResults();

                        var lowest =
                            LockSpaceSearcher.LockResults.OrderByDescending(x => x.Value.LinesRemoved)
                            //.ThenBy(x => x.Value.NumberOfHoles)
                                .ThenByDescending(x => x.Value.MaxHeight)                                
                                .ThenByDescending(x => x.Value.MinHeight)            
                                                    
                                .ThenByDescending(x => x.Value.MinDistanceFromCenter)                                
                                .First();
                        foreach (var d in lowest.Value.Directions.OrderBy(x => x.Count).First())
                        {
                            CurrentGameState.ExecuteMove(d);
                            //RefreshGameState();
                            Application.DoEvents();
                            //Thread.Sleep(100);
                        }
                        //Thread.Sleep(300);
                        RefreshGameState();
                    }
                }
                catch (GameOverException)
                {
                    outputs.Add(new Output
                    {
                        problemId = CurrentGameState.Problem.Id,
                        seed = CurrentGameState.Problem.SourceSeeds[i],
                        tag = string.Format("score_" + CurrentGameState.Score + "_" + CurrentGameState.Problem.SourceSeeds[i]),
                        solution = CurrentGameState.Moves.ToSolutionString()
                    });
                }
            }

            File.WriteAllText("out.json", JsonConvert.SerializeObject(outputs));
            SolutionBox.Text = "";
            foreach (var o in outputs)
            {
                SolutionBox.Text += o.tag + "\n";
            }
            //SolutionBox.Text = CurrentGameState.Moves.ToSolutionString();
        }

        private void SolutionBox_TextChanged(object sender, EventArgs e)
        {
            CurrentGameState.Reset(0);
            var directions = SolutionBox.Text.FromSolutionString();

            try
            {
                foreach (var direction in directions)
                {
                    CurrentGameState.ExecuteMove(direction);
                }
            }
            catch (GameOverException)
            {
            }
            RefreshGameState();
        }

        private void SolutionBox_SelectionChanged(object sender, EventArgs e)
        {
            CurrentGameState.Reset(0);
            var directions = SolutionBox.SelectedText.FromSolutionString();

            try
            {
                foreach (var direction in directions)
                {
                    CurrentGameState.ExecuteMove(direction);
                }
            }
            catch (GameOverException)
            {
            }
            RefreshGameState();
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