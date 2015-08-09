namespace Game.Core
{
    public enum CellState
    {
        None = 0,
        Filled = 1
    }

    public class BoardState
    {
        public CellState[,] Cells { get; }
        public Problem Problem { get; set; }


        public int Width => Problem.Width;
        public int Height => Problem.Height;

        public BoardState(Problem problem)
        {
            Problem = problem;
            Cells = new CellState[problem.Width, problem.Height];

            foreach (var cell in problem.Filled)
                Cells[cell.X, cell.Y] = CellState.Filled;
        }

        public int RemoveFullLines()
        {
            int removed = 0;
            for (int row = 0; row < Height; row++)
            {
                var full = true;
                for (int col = 0; col < Width; col++)
                {
                    full = full && Cells[col, row].HasFlag(CellState.Filled);
                }

                if (full)
                {
                    removed++;                    
                    for (int crow = row; crow > 0; crow--)
                    {
                        for (int ccol = 0; ccol < Width; ccol++)
                        {
                            Cells[ccol, crow] = Cells[ccol, crow - 1];
                        }
                    }
                    for (int ccol = 0; ccol < Width; ccol++)
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