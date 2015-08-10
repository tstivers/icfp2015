using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Game.Core;

namespace Game.Controllers
{
    public class NotSoGreatController
    {
        public delegate void ProgressCallback(GameState state);

        public ProgressCallback OnSolveStarted { get; set; }
        public ProgressCallback OnMove { get; set; }
        public ProgressCallback OnLock { get; set; }
        public ProgressCallback OnGameOver { get; set; }
        public ProgressCallback OnProblemSolved { get; set; }

        public GameState GameState { get; set; }

        public NotSoGreatController(Problem problem)
        {
            GameState = new GameState(problem, null);
        }

        public List<Output> Solve()
        {
            var outputs = new List<Output>();            

            OnSolveStarted?.Invoke(GameState);

            for (var i = 0; i < GameState.Problem.SourceSeeds.Length; i++)
            {
                GameState.Reset(i);
                try
                {
                    while (true)
                    {
                        var lockSpaceSearcher = new LockSpaceSearcher();
                        lockSpaceSearcher.GameState = GameState;
                        lockSpaceSearcher.GenerateLockResults();

                        var lowest =
                            lockSpaceSearcher.LockResults.OrderByDescending(x => x.Value.LinesRemoved)
                                .ThenBy(x => x.Value.NumberOfHoles)
                                .ThenByDescending(x => x.Value.MaxHeight)
                                .ThenByDescending(x => x.Value.MinHeight)
                                .ThenByDescending(x => x.Value.MinDistanceFromCenter)
                                .First();
                        foreach (var d in lowest.Value.Directions.OrderBy(x => x.Count).First())
                        {
                            GameState.ExecuteMove(d);
                            OnMove?.Invoke(GameState);
                        }

                        OnLock?.Invoke(GameState);
                    }
                }
                catch (GameOverException)
                {
                    outputs.Add(new Output
                    {
                        problemId = GameState.Problem.Id,
                        seed = GameState.Problem.SourceSeeds[i],
                        tag = string.Format("[{0}] {1}", DateTime.Now.ToShortTimeString(), GameState.Score),
                        solution = GameState.Moves.ToSolutionString()
                    });
                    OnGameOver?.Invoke(GameState);
                }
            }

            OnProblemSolved?.Invoke(GameState);

            return outputs;
        }
    }
}