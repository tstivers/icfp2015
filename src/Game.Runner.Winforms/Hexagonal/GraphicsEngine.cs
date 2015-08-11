using System;
using System.Drawing;

namespace Hexagonal
{
    public class GraphicsEngine
    {
        private Board board;
        private float boardPixelHeight;
        private float boardPixelWidth;
        private int boardXOffset;
        private int boardYOffset;

        public GraphicsEngine(Board board)
        {
            Initialize(board, 0, 0);
        }

        public GraphicsEngine(Board board, int xOffset, int yOffset)
        {
            Initialize(board, xOffset, yOffset);
        }

        public int BoardXOffset
        {
            get { return boardXOffset; }
            set { throw new NotImplementedException(); }
        }

        public int BoardYOffset
        {
            get { return boardYOffset; }
            set { throw new NotImplementedException(); }
        }

        private void Initialize(Board board, int xOffset, int yOffset)
        {
            this.board = board;
            boardXOffset = xOffset;
            boardYOffset = yOffset;
        }

        public void Draw(Graphics graphics)
        {
            var width = Convert.ToInt32(System.Math.Ceiling(board.PixelWidth));
            var height = Convert.ToInt32(System.Math.Ceiling(board.PixelHeight));
            // seems to be needed to avoid bottom and right from being chopped off
            width += 1;
            height += 1;

            //
            // Create drawing objects
            //
            var bitmap = new Bitmap(width, height);
            var bitmapGraphics = Graphics.FromImage(bitmap);
            var p = new Pen(Color.Black);
            var sb = new SolidBrush(Color.Black);


            //
            // Draw Board background
            //
            sb = new SolidBrush(board.BoardState.BackgroundColor);
            bitmapGraphics.FillRectangle(sb, 0, 0, width, height);

            //
            // Draw Hex Background 
            //
            for (var i = 0; i < board.Hexes.GetLength(0); i++)
            {
                for (var j = 0; j < board.Hexes.GetLength(1); j++)
                {
                    //bitmapGraphics.DrawPolygon(p, board.Hexes[i, j].Points);
                    bitmapGraphics.FillPolygon(new SolidBrush(board.Hexes[i, j].HexState.BackgroundColor),
                        board.Hexes[i, j].Points);
                }
            }


            //
            // Draw Hex Grid
            //
            p.Color = board.BoardState.GridColor;
            p.Width = board.BoardState.GridPenWidth;
            for (var i = 0; i < board.Hexes.GetLength(0); i++)
            {
                for (var j = 0; j < board.Hexes.GetLength(1); j++)
                {
                    bitmapGraphics.DrawPolygon(p, board.Hexes[i, j].Points);
                }
            }

            //
            // Draw Active Hex, if present
            //
            if (board.BoardState.ActiveHex != null)
            {
                sb = new SolidBrush(board.BoardState.ActiveHexBorderColor);
                p.Width = board.BoardState.ActiveHexBorderWidth;
                //bitmapGraphics.DrawPolygon(p, board.BoardState.ActiveHex.Points);
                bitmapGraphics.FillEllipse(sb,
                    new RectangleF(board.BoardState.ActiveHex.CenterPoint.X - 4,
                        board.BoardState.ActiveHex.CenterPoint.Y - 4, 8, 8));
            }

            //
            // Draw internal bitmap to screen
            //
            graphics.DrawImage(bitmap, new Point(boardXOffset, boardYOffset));

            //
            // Release objects
            //
            bitmapGraphics.Dispose();
            bitmap.Dispose();
        }
    }
}