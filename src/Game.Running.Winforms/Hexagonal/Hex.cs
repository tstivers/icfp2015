using System;
using System.Drawing;

namespace Hexagonal
{
    public class Hex
    {
        private float h;
        private HexState hexState;
        private PointF[] points;
        private float r;
        private float side;
        private float x;
        private float y;

        /// <param name="side">length of one side of the hexagon</param>
        public Hex(int x, int y, int side, HexOrientation orientation)
        {
            Initialize(Math.ConvertToFloat(x), Math.ConvertToFloat(y), Math.ConvertToFloat(side), orientation);
        }

        public Hex(float x, float y, float side, HexOrientation orientation)
        {
            Initialize(x, y, side, orientation);
        }

        public Hex(PointF point, float side, HexOrientation orientation)
        {
            Initialize(point.X, point.Y, side, orientation);
        }

        public Hex()
        {
        }

        public PointF CenterPoint
        {
            get { return new PointF(x, y + h + (Side/2)); }
        }

        public HexOrientation Orientation { get; set; }

        public PointF[] Points
        {
            get { return points; }
            set { }
        }

        public float Side
        {
            get { return side; }
            set { }
        }

        public float H
        {
            get { return h; }
            set { }
        }

        public float R
        {
            get { return r; }
            set { }
        }

        public HexState HexState
        {
            get { return hexState; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Sets internal fields and calls CalculateVertices()
        /// </summary>
        private void Initialize(float x, float y, float side, HexOrientation orientation)
        {
            this.x = x;
            this.y = y;
            this.side = side;
            Orientation = orientation;
            hexState = new HexState();
            CalculateVertices();
        }

        /// <summary>
        ///     Calculates the vertices of the hex based on orientation. Assumes that points[0] contains a value.
        /// </summary>
        private void CalculateVertices()
        {
            //  
            //  h = short length (outside)
            //  r = long length (outside)
            //  side = length of a side of the hexagon, all 6 are equal length
            //
            //  h = sin (30 degrees) x side
            //  r = cos (30 degrees) x side
            //
            //		 h
            //	     ---
            //   ----     |r
            //  /    \    |          
            // /      \   |
            // \      /
            //  \____/
            //
            // Flat orientation (scale is off)
            //
            //     /\
            //    /  \
            //   /    \
            //   |    |
            //   |    |
            //   |    |
            //   \    /
            //    \  /
            //     \/
            // Pointy orientation (scale is off)

            h = Math.CalculateH(side);
            r = Math.CalculateR(side);

            switch (Orientation)
            {
                case HexOrientation.Flat:
                    // x,y coordinates are top left point
                    points = new PointF[6];
                    points[0] = new PointF(x, y);
                    points[1] = new PointF(x + side, y);
                    points[2] = new PointF(x + side + h, y + r);
                    points[3] = new PointF(x + side, y + r + r);
                    points[4] = new PointF(x, y + r + r);
                    points[5] = new PointF(x - h, y + r);
                    break;
                case HexOrientation.Pointy:
                    //x,y coordinates are top center point
                    points = new PointF[6];
                    points[0] = new PointF(x, y);
                    points[1] = new PointF(x + r, y + h);
                    points[2] = new PointF(x + r, y + side + h);
                    points[3] = new PointF(x, y + side + h + h);
                    points[4] = new PointF(x - r, y + side + h);
                    points[5] = new PointF(x - r, y + h);
                    break;
                default:
                    throw new Exception("No HexOrientation defined for Hex object.");
            }
        }
    }
}