using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

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
        public GameState(Problem problem, IController controller)
        {
            Problem = problem;
            Controller = controller;
            Reset(0);
        }

        public Problem Problem { get; set; }
        public BoardState BoardState { get; set; }
        public UnitState CurrentUnitState { get; set; }
        public IController Controller { get; set; }
        private LinearCongruentGenerator LCG { get; set; }
        protected Unit CurrentUnit { get; private set; }
        protected Unit NextUnit { get; private set; }
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

            var x = (BoardState.Width/2) - (unitWidth/2) - min.X;
            var y = 0 - min.Y;
            var pos = new Point(x, y);

            var cells = new Point[unit.Members.Length];
            for (int i = 0; i < unit.Members.Length; i++)            
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
                Score += score;

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
            int removedLines = 0;
            for (int i = minHeight; i <= maxHeight; i++)
            {
                bool removed = true;
                for (int j = 0; j < BoardState.Width; j++)
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

        public int CountNumberOfHoles(Point[] cells)
        {
            var holes = 0;

            foreach (var cell in cells)
            {
                var se = cell.Y % 2 == 1
                            ? new Point(cell.X + 1, cell.Y + 1)
                            : new Point(cell.X, cell.Y + 1);
                var sw = cell.Y % 2 == 1
                            ? new Point(cell.X, cell.Y + 1)
                            : new Point(cell.X - 1, cell.Y + 1);

                if (se.X >= 0 && se.X < BoardState.Width && se.Y >= 0 && se.Y < BoardState.Height && !BoardState.Cells[se.X, se.Y].HasFlag(CellState.Filled) && !cells.Contains(se))
                    holes++;

                if (sw.X >= 0 && sw.X < BoardState.Width && sw.Y >= 0 && sw.Y < BoardState.Height && !BoardState.Cells[sw.X, sw.Y].HasFlag(CellState.Filled) && !cells.Contains(sw))
                    holes++;
            }

            return holes;
        }
    }

    public class GameOverException : Exception
    {
    }
}