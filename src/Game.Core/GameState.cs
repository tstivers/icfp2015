using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game.Core
{
    public enum Direction
    {
        E,
        W,
        SE,
        SW,
        CW,
        CCW
    }

    public class GameState
    {
        public GameState(Problem problem)
        {
            Problem = problem;            
            Reset(0);
        }

        public Problem Problem { get; set; }
        public BoardState BoardState { get; set; }
        public UnitState CurrentUnitState { get; set; }        
        private LinearCongruentGenerator LCG { get; set; }
        protected Unit CurrentUnit { get; private set; }
        protected Unit NextUnit { get; private set; }
        public int UnitsLeft { get; private set; }
        public int Score { get; private set; }
        public List<Direction> Moves { get; } = new List<Direction>();
        public int LastLinesCleared { get; set; }

        public void Reset(int gameNumber)
        {
            BoardState = new BoardState(Problem);
            LCG = new LinearCongruentGenerator(Problem.SourceSeeds[gameNumber]);
            UnitsLeft = Problem.SourceLength;
            Score = 0;
            NextUnit = Problem.Units[LCG.NextInt()%Problem.Units.Length];
            SpawnNextUnit();
            Moves.Clear();
        }

        public void SpawnNextUnit()
        {
            CurrentUnit = NextUnit;
            NextUnit = Problem.Units[LCG.NextInt()%Problem.Units.Length];
            UnitsLeft -= 1;

            if (UnitsLeft < 0)
            {
                CurrentUnit = null;
                throw new GameOverException();
            }

            // calc starting position for unit
            CurrentUnitState = CalcStartingPosition(CurrentUnit);

            if (UnitIsLocked(CurrentUnitState))
            {
                CurrentUnit = null;
                CurrentUnitState = null;
                throw new GameOverException();
            }
        }

        public UnitState CalcStartingPosition(Unit unit)
        {
            var max = new Cell();
            var min = new Cell(int.MaxValue, int.MaxValue);

            foreach (var cell in unit.Members)
            {
                max.X = Math.Max(cell.X, max.X);
                max.Y = Math.Max(cell.Y, max.Y);
                min.X = Math.Min(cell.X, min.X);
                min.Y = Math.Min(cell.Y, min.Y);
            }

            var unitWidth = 1 + (max.X - min.X);

            var x = (int) Math.Floor(BoardState.Width/2.0 - unitWidth/2.0) - min.X;
            var y = 0 - min.Y;
            var pos = new Point(x, y);

            var cells = new Point[unit.Members.Length];
            for (var i = 0; i < unit.Members.Length; i++)
                cells[i] = new Point(unit.Members[i].X + pos.X, unit.Members[i].Y + pos.Y);


            var position = new Position(new Point(CurrentUnit.Pivot.X + pos.X, CurrentUnit.Pivot.Y + pos.Y), 0);

            var state = new UnitState(position, cells);

            return state;
        }

        public bool MoveWillLock(Direction direction)
        {
            return UnitIsLocked(CurrentUnitState.Translate(direction));
        }

        public bool ExecuteMove(Direction direction)
        {
            Moves.Add(direction);

            // check to see if it was a lock
            if (MoveWillLock(direction))
            {
                // update board state
                foreach (var cell in CurrentUnitState.Cells)
                {
                    BoardState.Cells[cell.X, cell.Y] = CellState.Filled;
                }

                // check for removed lines
                var ls = BoardState.RemoveFullLines();

                // calc score
                var size = CurrentUnit.Members.Length;
                var score = size + 100*(1 + ls)*ls/2;
                score += LastLinesCleared > 0 ? (int)Math.Floor((LastLinesCleared - 1.0)*score/10.0) : 0;
                Score += score;
                LastLinesCleared = ls;

                // validate game state
                // spawn next piece                 
                SpawnNextUnit();
            }
            else
            {
                CurrentUnitState = CurrentUnitState.Translate(direction);
            }
            return true;
        }

        public bool ExecuteMove(char c)
        {
            return true;
        }

        public bool UnitIsLocked(UnitState state)
        {
            foreach (var cell in state.Cells)
            {
                if (cell.X < 0 || cell.X >= BoardState.Width)
                    return true;

                if (cell.Y < 0 || cell.Y >= BoardState.Height)
                    return true;

                if (BoardState.Cells[cell.X, cell.Y].HasFlag(CellState.Filled))
                    return true;
            }

            return false;
        }

        public int CheckRemovedLines(Point[] cells, int minHeight, int maxHeight)
        {
            var removedLines = 0;
            for (var i = minHeight; i <= maxHeight; i++)
            {
                var removed = true;
                for (var j = 0; j < BoardState.Width; j++)
                {
                    if (!BoardState.Cells[j, i].HasFlag(CellState.Filled) && !cells.Contains(new Point(j, i)))
                    {
                        removed = false;
                        break;
                    }
                }

                if (removed)
                    removedLines++;
            }

            return removedLines;
        }

        public Point[] GetNeighboringPoints(Point cell)
        {
            var points = new Point[6];
            points[0] = cell.Y % 2 == 1
                    ? new Point(cell.X + 1, cell.Y + 1)
                    : new Point(cell.X, cell.Y + 1);
            points[1] = cell.Y % 2 == 1
                    ? new Point(cell.X, cell.Y + 1)
                    : new Point(cell.X - 1, cell.Y + 1);
            points[2] = new Point(cell.X -1, cell.Y);
            points[3] = new Point(cell.X + 1, cell.Y);
            points[4] = cell.Y % 2 == 1
                    ? new Point(cell.X + 1, cell.Y - 1)
                    : new Point(cell.X, cell.Y - 1);
            points[5] = cell.Y % 2 == 1
                    ? new Point(cell.X, cell.Y - 1)
                    : new Point(cell.X - 1, cell.Y - 1);

            return points;
        }

        public int CountNumberOfHoles(Point[] cells)
        {
            var holes = 0;

            foreach (var cell in cells)
            {
                foreach (var n in GetNeighboringPoints(cell))
                {
                    if (n.X >= 0 && n.X < BoardState.Width && n.Y >= 0 && n.Y < BoardState.Height &&
                        !BoardState.Cells[n.X, n.Y].HasFlag(CellState.Filled) && !cells.Contains(n))
                        holes++;
                }
            }

            return holes;
        }
    }

    public class GameOverException : Exception
    {
    }
}