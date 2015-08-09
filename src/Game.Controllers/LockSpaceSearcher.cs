using System;
using System.Collections.Generic;
using System.Drawing;
using Game.Core;

namespace Game.Controllers
{
    public class LockSpaceSearcher
    {
        public GameState GameState { get; set; }
        public Dictionary<PointF, LockResult> LockResults { get; set; } = new Dictionary<PointF, LockResult>();

        public void GenerateLockResults()
        {
            var closed_set = new HashSet<PointF>();
            var came_from = new Dictionary<PointF, PointF>();
            var open_set = new Queue<PointF>();
            var open_set_hash = new HashSet<PointF>();
            open_set.Enqueue(GameState.CurrentUnitPosition);

            while (open_set.Count != 0)
            {
                var current = open_set.Dequeue();

                if (GameState.MoveIsLocked(current))
                    ReconstructPath(came_from, GameState.CurrentUnitPosition, current);
                else
                {
                    closed_set.Add(current);
                    foreach (var neighbor in GetNeighbors(current))
                    {
                        if (!closed_set.Contains(neighbor) && !open_set_hash.Contains(neighbor))
                        {
                            open_set.Enqueue(neighbor);
                            open_set_hash.Add(neighbor);
                            came_from[neighbor] = current;
                        }
                    }
                }
            }
        }

        public void ReconstructPath(Dictionary<PointF, PointF> came_from, PointF start, PointF current)
        {
            LockResult result;
            if (!LockResults.TryGetValue(came_from[current], out result))
            {
                result = new LockResult();
                LockResults[came_from[current]] = result;
            }

            var movelist = new List<Direction>();


            while (current != start)
            {
                var prev = came_from[current];

                if (prev.X == current.X + 1)
                    movelist.Add(Direction.W);
                else if (prev.X == current.X - 1)
                    movelist.Add(Direction.E);
                else if (prev.X == current.X + 0.5f)
                    movelist.Add(Direction.SW);
                else if (prev.X == current.X - 0.5f)
                    movelist.Add(Direction.SE);
                else
                {
                    throw new Exception();
                }

                current = prev;
            }

            movelist.Reverse();
            result.Directions.Add(movelist);
        }

        public IEnumerable<PointF> GetNeighbors(PointF current)
        {
            return new List<PointF>
            {
                new PointF(current.X + 1, current.Y),
                new PointF(current.X - 1, current.Y),
                new PointF(current.X + 0.5f, current.Y + 1),
                new PointF(current.X - 0.5f, current.Y + 1)
            };
        }
    }
}