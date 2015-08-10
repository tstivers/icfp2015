using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Core
{
    public class RotationHelper
    {
        public static Dictionary<Point, Point> Matrix = new Dictionary<Point, Point>
        {
            {new Point(-1, -1), new Point(2, -1)},
            {new Point(0, -1), new Point(1, 0)},
            {new Point(1, -1), new Point(0, 1)},
            {new Point(2, -1), new Point(0, 2)},

            {new Point(-2, 0), new Point(1, -2)},
            {new Point(-1, 0), new Point(1, -1)},
            {new Point(0, 0), new Point(0, 0)},
            {new Point(1, 0), new Point(0, 1)},
            {new Point(2, 0), new Point(-1, 2)},

            {new Point(-1, 1), new Point(0, -2)},
            {new Point(0, 1), new Point(-1, -1)},
            {new Point(1, 1), new Point(-1, 0)},
            {new Point(2, 1), new Point(-2, -1)}
        };
    }

    public class UnitState
    {
        public UnitState(Position position, Point[] cells)
        {
            Position = position;
            Cells = cells;
        }

        public Position Position { get; }
        public Point[] Cells { get; }

        public UnitState Translate(Direction direction)
        {
            return new UnitState(Position.Translate(direction), GetTranslatedCells(direction));
        }

        public Point[] GetTranslatedCells(Direction direction)
        {
            var translated = new Point[Cells.Length];
            for (var i = 0; i < Cells.Length; i++)
            {
                switch (direction)
                {
                    case Direction.E:
                        translated[i] = new Point(Cells[i].X + 1, Cells[i].Y);
                        break;
                    case Direction.SE:
                        translated[i] = Cells[i].Y%2 == 1
                            ? new Point(Cells[i].X + 1, Cells[i].Y + 1)
                            : new Point(Cells[i].X, Cells[i].Y + 1);
                        break;
                    case Direction.SW:
                        translated[i] = Cells[i].Y%2 == 1
                            ? new Point(Cells[i].X, Cells[i].Y + 1)
                            : new Point(Cells[i].X - 1, Cells[i].Y + 1);
                        break;
                    case Direction.W:
                        translated[i] = new Point(Cells[i].X - 1, Cells[i].Y);
                        break;
                    case Direction.CW:
                    {
                        var cell = Cells[i];
                        var pivot = Position.Location;
                        var local = new Point(cell.X - pivot.X, cell.Y - pivot.Y);

                        if ((pivot.Y & 1) == 1)
                        {
                            var xx = local.X - (local.Y + (local.Y & 1))/2;
                            var zz = local.Y;
                            var yy = -xx - zz;

                            var dx = -zz;
                            var dy = -xx;
                            var dz = -yy;

                            translated[i] = new Point(pivot.X + (dx + (dz + (dz & 1))/2), pivot.Y + dz);
                        }
                        else
                        {
                            var xx = local.X - (local.Y - (local.Y & 1))/2;
                            var zz = local.Y;
                            var yy = -xx - zz;

                            var dx = -zz;
                            var dy = -xx;
                            var dz = -yy;

                            translated[i] = new Point(pivot.X + (dx + (dz - (dz & 1))/2), pivot.Y + dz);
                        }
                    }
                        break;
                    case Direction.CCW:
                    {
                        var cell = Cells[i];
                        var pivot = Position.Location;
                        var local = new Point(cell.X - pivot.X, cell.Y - pivot.Y);

                        if ((pivot.Y & 1) == 1)
                        {
                            var xx = local.X - (local.Y + (local.Y & 1))/2;
                            var zz = local.Y;
                            var yy = -xx - zz;

                            var dx = -yy;
                            var dy = -zz;
                            var dz = -xx;

                            translated[i] = new Point(pivot.X + (dx + (dz + (dz & 1))/2), pivot.Y + dz);
                        }
                        else
                        {
                            var xx = local.X - (local.Y - (local.Y & 1))/2;
                            var zz = local.Y;
                            var yy = -xx - zz;

                            var dx = -yy;
                            var dy = -zz;
                            var dz = -xx;

                            translated[i] = new Point(pivot.X + (dx + (dz - (dz & 1))/2), pivot.Y + dz);
                        }
                    }
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return translated;
        }
    }

    public class Position
    {
        public Position(Point location, int orientation)
        {
            Location = location;
            Orientation = orientation;
        }

        public Point Location { get; }
        public int Orientation { get; }

        protected bool Equals(Position other)
        {
            return Location.Equals(other.Location) && Orientation == other.Orientation;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Location.GetHashCode()*397) ^ Orientation;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !Equals(left, right);
        }

        public Position Translate(Direction direction)
        {
            switch (direction)
            {
                case Direction.E:
                    return new Position(new Point(Location.X + 1, Location.Y), Orientation);
                case Direction.SE:
                    return new Position(Location.Y%2 == 1
                        ? new Point(Location.X + 1, Location.Y + 1)
                        : new Point(Location.X, Location.Y + 1), Orientation);
                case Direction.SW:
                    return new Position(Location.Y%2 == 1
                        ? new Point(Location.X, Location.Y + 1)
                        : new Point(Location.X - 1, Location.Y + 1), Orientation);
                case Direction.W:
                    return new Position(new Point(Location.X - 1, Location.Y), Orientation);
                case Direction.CW:
                    return new Position(new Point(Location.X, Location.Y), (Orientation + 1)%6);
                case Direction.CCW:
                    return new Position(new Point(Location.X, Location.Y), Orientation > 0 ? (Orientation - 1) : 5);

                default:
                    throw new ArgumentException();
            }
        }
    }
}