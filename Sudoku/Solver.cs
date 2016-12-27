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
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (try2Solve(row, col))
                    {
                        row = 0;
                        col = 0;
                    }
                }
            }
            return _grid;
        }
        #endregion

        #region Helpers
        private int determineSquare(int row, int column)
        {
            int square = ((row / 3) * 3) + (column / 3);
            return square;
        }

        private bool try2Solve(int row, int column)
        {
            int[,] numbers = _grid.getData();
            List<int> possibleValuesFound = new List<int>();
            if (numbers[row, column].Equals(0))
            {
                for (int possiblevalues = 1; possiblevalues <= 9; possiblevalues++)
                {
                    if (!_grid.rowContains(row, possiblevalues))
                    {
                        if (!_grid.colContains(column,possiblevalues))
                        {
                            if (!_grid.squareContains(determineSquare(row, column),possiblevalues))
                            {
                                possibleValuesFound.Add(possiblevalues);
                            }
                        }
                    }
                }

                if (possibleValuesFound.Count == 1)
                {
                    numbers[row, column] = possibleValuesFound[0];
                    _grid.setData(numbers);
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
