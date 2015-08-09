using System.Runtime.CompilerServices;

namespace Game.Core
{
    public class Cell
    {
        public Cell() { }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static Cell operator+(Cell lhs, Cell rhs)
        {
            return new Cell(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
    }
}