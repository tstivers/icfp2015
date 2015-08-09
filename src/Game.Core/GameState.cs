using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
        private static readonly SizeF OffsetE = new SizeF(1, 0);
        private static readonly SizeF OffsetW = new SizeF(-1, 0);
        private static readonly SizeF OffsetSe = new SizeF(0.5f, 1);
        private static readonly SizeF OffsetSw = new SizeF(-0.5f, 1);

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
        public Unit CurrentUnit { get; private set; }
        public Unit NextUnit { get; private set; }
        public PointF CurrentUnitPosition { get; set; }
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

        public IEnumerable<Cell> GetCurrentUnitCells(float dx, float dy)
        {
            foreach (var cell in CurrentUnit.Members)
            {
                var x = cell.X + (int) (dx + (cell.Y/2.0));
                var y = cell.Y + (int) dy;
                yield return new Cell(x, y);
            }
        }

        public PointF CalcStartingPosition(Unit unit)
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
            var pos = new PointF(x, y);
            if (MoveIsLocked(pos))
            {
                CurrentUnit = null;
                throw new GameOverException();
            }

            return pos;
        }

        public bool MoveWillLock(Direction direction)
        {
            var offset = GetOffset(direction);
            var x = CurrentUnitPosition.X + offset.Width;
            var y = CurrentUnitPosition.Y + offset.Height;

            foreach (var cell in GetCurrentUnitCells(x, y))
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

        public static SizeF GetOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.E:
                    return OffsetE;
                case Direction.SE:
                    return OffsetSe;
                case Direction.SW:
                    return OffsetSw;
                case Direction.W:
                    return OffsetW;
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
                foreach (var cell in GetCurrentUnitCells(CurrentUnitPosition.X, CurrentUnitPosition.Y))
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
                CurrentUnitPosition += GetOffset(direction);
            }
            return true;
        }

        public bool ExecuteMove(char c)
        {
            return true;
        }

        public bool MoveIsLocked(PointF current)
        {
            foreach (var cell in GetCurrentUnitCells(current.X, current.Y))
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