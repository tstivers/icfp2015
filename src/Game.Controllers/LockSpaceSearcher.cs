using System;
using System.Collections.Generic;
using System.Drawing;
using Game.Core;

namespace Game.Controllers
{
    public class UnitState
    {
        public Point pos { get; set; }
        public Point[] Cells { get; set; }
    }

    public class LockSpaceSearcher
    {
        public GameState GameState { get; set; }
        public Dictionary<Point, LockResult> LockResults { get; set; } = new Dictionary<Point, LockResult>();

        public void GenerateLockResults()
        {
            var closed_set = new HashSet<Point>();
            var came_from = new Dictionary<Point, Point>();
            var open_set = new Queue<UnitState>();
            var open_set_hash = new HashSet<Point>();
            open_set.Enqueue(new UnitState() {pos =  GameState.CurrentUnitPosition, Cells = GameState.CurrentUnitCells});

            while (open_set.Count != 0)
            {
                var current = open_set.Dequeue();

                if (GameState.CellsAreLocked(current.Cells))
                    ReconstructPath(came_from, GameState.CurrentUnitPosition, current.pos);
                else
                {
                    closed_set.Add(current.pos);
                    foreach (var neighbor in GetNeighbors(current))
                    {
                        if (!closed_set.Contains(neighbor.pos) && !open_set_hash.Contains(neighbor.pos))
                        {
                            open_set.Enqueue(neighbor);
                            open_set_hash.Add(neighbor.pos);
                            came_from[neighbor.pos] = current.pos;
                        }
                    }
                }
            }
        }

        public void ReconstructPath(Dictionary<Point, Point> came_from, Point start, Point current)
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

                if (current == GameState.GetNewPos(prev, Direction.E))
                    movelist.Add(Direction.E);
                else if (current == GameState.GetNewPos(prev, Direction.W))
                    movelist.Add(Direction.W);
                else if (current == GameState.GetNewPos(prev, Direction.SE))
                    movelist.Add(Direction.SE);
                else if (current == GameState.GetNewPos(prev, Direction.SW))
                    movelist.Add(Direction.SW);
                else
                {
                    throw new Exception();
                }

                current = prev;
            }

            movelist.Reverse();
            result.Directions.Add(movelist);
        }

        public IEnumerable<UnitState> GetNeighbors(UnitState current)
        {
            return new List<UnitState>
            {
                GetNewState(current, Direction.E),
                GetNewState(current, Direction.W),
                GetNewState(current, Direction.SE),
                GetNewState(current, Direction.SW),
            };
        }

        public UnitState GetNewState(UnitState current, Direction direction)
        {
            return new UnitState
            {
                pos = GameState.GetNewPos(current.pos, direction),
                Cells = GameState.GetTranslatedPoints(current.Cells, direction)
            };
        }
    }
}