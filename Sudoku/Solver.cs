using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solver
    {
        private SudokuGrid _grid;
        private SudokuGrid _solution;

        #region constructors
        public Solver()
        {
            _grid = new SudokuGrid();
        }

        public Solver (SudokuGrid grid)
        {
            _grid = grid;
        }

        public Solver(int[,] numbers)
        {
            _grid = new SudokuGrid(numbers);
        }
        #endregion

        #region Main Functions
        public SudokuGrid solve()
        {
            _solution = new SudokuGrid();
            if (solveRecursion() == true)
            {
                _grid = _solution;
            }
            return _grid;
        }

        private bool solveRecursion()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (_solution.getData()[row, col] != 0) continue;

                    List<int> availableValues = GetAvailableValues(row, col);

                    do
                    {
                        if (availableValues.Count == 0)
                        {
                            _solution.getData()[row, col] = 0;
                            return false;
                        }

                        _solution.getData()[row, col] = availableValues[0];
                        availableValues.RemoveAt(0);
                    } while (this.solveRecursion() == false); // Rekursiver Aufruf
                }
            }
            return true;
        }

        public void setGrid(SudokuGrid grid)
        {
            if (grid.isValid(false))
            {
                _grid = grid;
            }
        }
        #endregion

        #region Helpers
        private int determineSquare(int row, int column)
        {
            int square = ((row / 3) * 3) + (column / 3);
            return square;
        }

        private List<int> GetAvailableValues(int row, int col)
        {
            List<int> values = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //Alle Werte entfernen, die im Quadrat bereits vorhanden sind
            int square = DetermineSquare(row, col);
            for (int sqrRow = (square / 3) * 3; sqrRow < (square / 3) * 3 + 3; sqrRow++)
            {
                for (int sqrCol = (square % 3) * 3; sqrCol < (square % 3) * 3 + 3; sqrCol++)
                {
                    if (Solution[sqrRow, sqrCol] != 0) values.Remove(Solution[sqrRow, sqrCol]);
                }
            }

            //Alle Werte entfernen, die in der Reihe bereits vorhanden sind
            for (int rowCol = 0; rowCol < 9; rowCol++)
            {
                if (Solution[row, rowCol] != 0) values.Remove(Solution[row, rowCol]);
            }

            //Alle Werte entfernen, die in der Zeile vorhanden sind
            for (int colRow = 0; colRow < 9; colRow++)
            {
                if (Solution[colRow, col] != 0) values.Remove(Solution[colRow, col]);
            }

            values = Shuffle(values);

            return values;
        }
        #endregion
    }
}
