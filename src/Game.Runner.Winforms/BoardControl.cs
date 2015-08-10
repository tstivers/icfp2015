using System.Drawing;
using System.Windows.Forms;
using Game.Core;
using Hexagonal;

namespace Game.Running.Winforms
{
    public partial class BoardControl : Control
    {
        private GameState _gameState;

        public BoardControl()
        {
            InitializeComponent();
        }

        public GameState GameState
        {
            get { return _gameState; }
            set
            {
                _gameState = value;
                if (_gameState != null) ResetGameState();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        public Board Board { get; set; }
        public GraphicsEngine GraphicsEngine { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (_gameState != null)
            {
                var board = _gameState.BoardState;
                for (var col = 0; col < board.Height; col++)
                    for (var row = 0; row < board.Width; row++)
                        Board.Hexes[col, row].HexState.BackgroundColor = board.Cells[row, col]
                            ? Color.OrangeRed
                            : DefaultBackColor;

                if (_gameState.CurrentUnitState != null)
                {
                    foreach (var cell in _gameState.CurrentUnitState.Cells)
                    {
                        Board.Hexes[cell.Y, cell.X]
                            .HexState.BackgroundColor = Color.Aqua;
                    }

                    var pivot = _gameState.CurrentUnitState.Position.Location;

                    if (pivot.X >= 0 && pivot.X < _gameState.BoardState.Width && pivot.Y >= 0 &&
                        pivot.Y < _gameState.BoardState.Height)
                        Board.BoardState.ActiveHex = Board.Hexes[pivot.Y, pivot.X];
                    else
                        Board.BoardState.ActiveHex = null;
                }

                GraphicsEngine?.Draw(pe.Graphics);
            }
        }

        public void ResetGameState()
        {
            Board = new Board(_gameState.Problem.Width, _gameState.Problem.Height, 15, HexOrientation.Pointy);
            GraphicsEngine = new GraphicsEngine(Board);
            Refresh();
        }
    }
}