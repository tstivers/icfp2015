using System.Drawing;
using System.Windows.Forms;
using Game.Core;
using Hexagonal;
using Board = Hexagonal.Board;

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

        public Board Board { get; set; }
        public GraphicsEngine GraphicsEngine { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_gameState != null)
            {
                var board = _gameState.BoardState;
                for (var col = 0; col < board.Height; col++)
                    for (var row = 0; row < board.Width; row++)
                        Board.Hexes[col, row].HexState.BackgroundColor = board.Cells[row, col].HasFlag(CellState.Filled)
                            ? Color.OrangeRed
                            : DefaultBackColor;

                if (_gameState.CurrentUnit != null)
                {
                    foreach (var cell in _gameState.GetCurrentUnitCells(_gameState.CurrentUnitPosition.X, _gameState.CurrentUnitPosition.Y))
                    {                        
                        Board.Hexes[cell.Y, cell.X]
                            .HexState.BackgroundColor = Color.Aqua;
                    }

                    Board.BoardState.ActiveHexBorderColor = Color.Crimson;
                    Board.BoardState.ActiveHex =
                        Board.Hexes[
                            _gameState.CurrentUnit.Pivot.Y + (int)_gameState.CurrentUnitPosition.Y,
                            _gameState.CurrentUnit.Pivot.X + (int)_gameState.CurrentUnitPosition.X];
                }


                GraphicsEngine?.Draw(pe.Graphics);
            }
        }

        public void ResetGameState()
        {
            Board = new Board(_gameState.Problem.Width, _gameState.Problem.Height, 5, HexOrientation.Pointy);
            GraphicsEngine = new GraphicsEngine(Board);
            Refresh();
        }
    }
}