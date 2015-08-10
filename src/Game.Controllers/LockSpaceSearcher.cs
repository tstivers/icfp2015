using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;

namespace Game.Controllers
{
    public class LockSpaceSearcher
    {
        public GameState GameState { get; set; }
        public Dictionary<Position, LockResult> LockResults { get; set; } = new Dictionary<Position, LockResult>();

        public void GenerateLockResults()
        {
            var closed_set = new HashSet<Position>();
            var came_from = new Dictionary<Position, UnitState>();
            var open_set = new Queue<UnitState>();
            var open_set_hash = new HashSet<Position>();
            open_set.Enqueue(GameState.CurrentUnitState);

            while (open_set.Count != 0)
            {
                var current = open_set.Dequeue();

                if (GameState.UnitIsLocked(current))
                    ReconstructPath(came_from, GameState.CurrentUnitState.Position, current);
                else
                {
                    closed_set.Add(current.Position);
                    foreach (var neighbor in GetNeighbors(current))
                    {
                        if (!closed_set.Contains(neighbor.Position) && !open_set_hash.Contains(neighbor.Position))
                        {
                            open_set.Enqueue(neighbor);
                            open_set_hash.Add(neighbor.Position);
                            came_from[neighbor.Position] = current;
                        }
                    }
                }
            }
        }

        public void ReconstructPath(Dictionary<Position, UnitState> came_from, Position start, UnitState currentState)
        {
            LockResult result;
            var currentPosition = currentState.Position;

            if (!LockResults.TryGetValue(came_from[currentPosition].Position, out result))
            {
                result = new LockResult();
                var prevState = came_from[currentPosition];
                LockResults[prevState.Position] = result;
                result.MaxHeight = prevState.Cells.OrderByDescending(x => x.Y).First().Y;
                result.MinHeight = prevState.Cells.OrderBy(x => x.Y).First().Y;
                result.LinesRemoved = GameState.CheckRemovedLines(prevState.Cells, result.MinHeight, result.MaxHeight);
                result.MinDistanceFromCenter =
                    prevState.Cells.Select(x => Math.Abs((GameState.BoardState.Width/2) - x.X)).Min();
                result.NumberOfOpenNeighbors = GameState.CountNumberOfOpenNeighbors(prevState.Cells);
            }

            var movelist = new List<Direction>();

            while (currentPosition != start)
            {
                var prev = came_from[currentPosition].Position;

                if (currentPosition == prev.Translate(Direction.E))
                    movelist.Add(Direction.E);
                else if (currentPosition == prev.Translate(Direction.W))
                    movelist.Add(Direction.W);
                else if (currentPosition == prev.Translate(Direction.SE))
                    movelist.Add(Direction.SE);
                else if (currentPosition == prev.Translate(Direction.SW))
                    movelist.Add(Direction.SW);
                else if (currentPosition == prev.Translate(Direction.CW))
                    movelist.Add(Direction.CW);
                else if (currentPosition == prev.Translate(Direction.CCW))
                    movelist.Add(Direction.CCW);
                else
                {
                    throw new Exception();
                }

                currentPosition = prev;
            }

            movelist.Reverse();
            result.Directions.Add(movelist);
        }

        public IEnumerable<UnitState> GetNeighbors(UnitState current)
        {
            return new List<UnitState>
            {
                current.Translate(Direction.E),
                current.Translate(Direction.W),
                current.Translate(Direction.SE),
                current.Translate(Direction.SW),
                current.Translate(Direction.CW),
                current.Translate(Direction.CCW)
            };
        }
    }
}