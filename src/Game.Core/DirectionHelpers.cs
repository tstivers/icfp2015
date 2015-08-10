using System.Collections.Generic;
using System.Text;

namespace Game.Core
{
    public static class DirectionHelpers
    {
        public static string ToSolutionString(this List<Direction> directions)
        {
            var sb = new StringBuilder(directions.Count);
            foreach (var d in directions)
            {
                switch (d)
                {
                    case Direction.E:
                        sb.Append('e');
                        break;
                    case Direction.SE:
                        sb.Append('m');
                        break;
                    case Direction.SW:
                        sb.Append('i');
                        break;
                    case Direction.W:
                        sb.Append('p');
                        break;
                    case Direction.CW:
                        sb.Append('d');
                        break;
                    case Direction.CCW:
                        sb.Append('k');
                        break;
                }
            }

            return sb.ToString();
        }

        public static List<Direction> FromSolutionString(this string str)
        {
            var directions = new List<Direction>();
            foreach (var c in str.ToLower())
            {
                switch (c)
                {
                    case 'p':
                    case '\'':
                    case '!':
                    case '.':
                    case '0':
                    case '3':
                        directions.Add(Direction.W);
                        break;
                    case 'b':
                    case 'c':
                    case 'e':
                    case 'f':
                    case 'y':
                    case '2':
                        directions.Add(Direction.E);
                        break;
                    case 'a':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case '4':
                        directions.Add(Direction.SW);
                        break;
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case ' ':
                    case '5':
                        directions.Add(Direction.SE);
                        break;
                    case 'd':
                    case 'q':
                    case 'r':
                    case 'v':
                    case 'z':
                    case '1':
                        directions.Add(Direction.CW);
                        break;
                    case 'k':
                    case 's':
                    case 't':
                    case 'u':
                    case 'w':
                    case 'x':
                        directions.Add(Direction.CCW);
                        break;
                }
            }

            return directions;
        }
    }
}