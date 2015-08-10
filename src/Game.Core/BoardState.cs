namespace Game.Core
{
    public enum CellState
    {
        None = 0,
        Filled = 1
    }

    public class BoardState
    {
        public BoardState(Problem problem)
        {
            Problem = problem;
            Cells = new CellState[problem.Width, problem.Height];

            foreach (var cell in problem.Filled)
                Cells[cell.X, cell.Y] = CellState.Filled;
        }

        public CellState[,] Cells { get; }
        public Problem Problem { get; set; }
        public int Width => Problem.Width;
        public int Height => Problem.Height;

        public int RemoveFullLines()
        {
            var removed = 0;
            for (var row = 0; row < Height; row++)
            {
                var full = true;
                for (var col = 0; col < Width; col++)
                {
                    full = full && Cells[col, row].HasFlag(CellState.Filled);
                }

                if (full)
                {
                    removed++;
                    for (var crow = row; crow > 0; crow--)
                    {
                        for (var ccol = 0; ccol < Width; ccol++)
                        {
                            Cells[ccol, crow] = Cells[ccol, crow - 1];
                        }
                    }
                    for (var ccol = 0; ccol < Width; ccol++)
                    {
                        Cells[ccol, 0] = CellState.None;
                    }
                    row--;
                }
            }

            return removed;
        }
    }
}