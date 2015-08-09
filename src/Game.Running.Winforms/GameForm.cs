using System;
using System.IO;
using System.Linq;
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

        public GameState CurrentGameState { get; set; }
        public LockSpaceSearcher LockSpaceSearcher { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var problem = Problem.FromFile(openFileDialog1.FileName);
                CurrentGameState = new GameState(problem, null);
                boardControl1.GameState = CurrentGameState;
                this.Text = openFileDialog1.FileName;
                
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
            try
            {
                while (true)
                {
                    LockSpaceSearcher = new LockSpaceSearcher();
                    LockSpaceSearcher.GameState = CurrentGameState;
                    LockSpaceSearcher.GenerateLockResults();

                    var lowest = LockSpaceSearcher.LockResults.OrderBy(x => -x.Key.Y).First();
                    foreach (var d in lowest.Value.Directions[0])
                    {
                        CurrentGameState.ExecuteMove(d);
                    }
                    
                }
            }
            catch (GameOverException)
            {
                RefreshGameState();
                var output = new Output
                {
                    problemId = CurrentGameState.Problem.Id,
                    seed = CurrentGameState.Problem.SourceSeeds[0],
                    tag = string.Format("score_" + CurrentGameState.Score),
                    solution = CurrentGameState.Moves.ToSolutionString()
                };

                File.WriteAllText("out.json", JsonConvert.SerializeObject(new [] { output} ));
                SolutionBox.Text = CurrentGameState.Moves.ToSolutionString();
            }
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
    }
}