using System.Drawing;

namespace Hexagonal
{
    public class BoardState
    {
        public BoardState()
        {
            BackgroundColor = Color.White;
            GridColor = Color.Black;
            GridPenWidth = 1;
            ActiveHex = null;
            ActiveHexBorderColor = Color.Blue;
            ActiveHexBorderWidth = 1;
        }

        #region Properties

        public Color BackgroundColor { get; set; }

        public Color GridColor { get; set; }

        public int GridPenWidth { get; set; }

        public Hex ActiveHex { get; set; }

        public Color ActiveHexBorderColor { get; set; }

        public int ActiveHexBorderWidth { get; set; }

        #endregion
    }
}