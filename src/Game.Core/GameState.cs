using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Core
{
    public enum Direction
    {
        E,
        W,
        SE,
        SW
    }

    public class GameState
    {
        public GameState(Problem problem, IController controller)
        {
            Problem = problem;
            Controller = controller;
            Reset(0);
        }

        public Problem Problem { get; set; }
        public BoardState BoardState { get; set; }
        public IController Controller { get; set; }
        private LinearCongruentGenerator LCG { get; set; }
        protected Unit CurrentUnit { get; private set; }
        protected Unit NextUnit { get; private set; }
        public Point CurrentUnitPosition { get; set; }
        public Point[] CurrentUnitCells { get; set; }
        public int UnitsLeft { get; private set; }
        public int Score { get; private set; }
        public List<Direction> Moves { get; } = new List<Direction>();

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
            CurrentUnitPosition = CalcStartingPosition(CurrentUnit);
        }

        public Point CalcStartingPosition(Unit unit)
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

            var x = (int) Math.Floor(BoardState.Width/2.0 - unitWidth/2.0);
            var y = 0 - min.Y;
            var pos = new Point(x, y);

            CurrentUnitCells = new Point[unit.Members.Length];
            for (int i = 0; i < unit.Members.Length; i++)            
                CurrentUnitCells[i] = new Point(unit.Members[i].X + pos.X, unit.Members[i].Y + pos.Y);            

            if (BoardIsLocked())
            {
                CurrentUnit = null;
                CurrentUnitCells = null;
                throw new GameOverException();
            }

            return pos;
        }

        public bool MoveWillLock(Direction direction)
        {            
            var newCells = GetTranslatedPoints(CurrentUnitCells, direction);

            foreach (var cell in newCells)
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

        public Point[] GetTranslatedPoints(Point[] points, Direction direction)
        {
            var translated = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
                translated[i] = GetNewPos(points[i], direction);

            return translated;
        }

        public static Point GetNewPos(Point current, Direction direction)
        {
            switch (direction)
            {
                case Direction.E:
                    return new Point(current.X + 1, current.Y);
                case Direction.SE:
                    return current.Y%2 == 1
                        ? new Point(current.X + 1, current.Y + 1)
                        : new Point(current.X, current.Y + 1);
                case Direction.SW:
                    return current.Y%2 == 1
                        ? new Point(current.X, current.Y + 1)
                        : new Point(current.X - 1, current.Y + 1);
                case Direction.W:
                    return new Point(current.X - 1, current.Y);
                default:
                    throw new ArgumentException();
            }
        }

        public bool ExecuteMove(Direction direction)
        {
            Moves.Add(direction);

            // check to see if it was a lock
            if (MoveWillLock(direction))
            {
                // update board state
                foreach (var cell in CurrentUnitCells)
                {
                    BoardState.Cells[cell.X, cell.Y] = CellState.Filled;
                }

                // check for removed lines
                var ls = BoardState.RemoveFullLines();

                // calc score
                var size = CurrentUnit.Members.Length;
                var score = size + 100*(1 + ls)*ls/2;
                Score += score;

                // validate game state
                // spawn next piece                 
                SpawnNextUnit();
            }
            else
            {
                CurrentUnitPosition = GetNewPos(CurrentUnitPosition, direction);
                CurrentUnitCells = GetTranslatedPoints(CurrentUnitCells, direction);
            }
            return true;
        }

        public bool ExecuteMove(char c)
        {
            return true;
        }

        public bool BoardIsLocked()
        {
            return CellsAreLocked(CurrentUnitCells);
        }

        public bool CellsAreLocked(Point[] cells)
        {
            foreach (var cell in cells)
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
    }

    public class GameOverException : Exception
    {
    }
}