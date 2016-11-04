using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
    class SudokuGrid
    {
        private int[,] data;

        #region Constructors
        public SudokuGrid()
        {
            new SudokuGrid(new int[9, 9]);
        }

        public SudokuGrid(int[,] myData)
        {
            this.data = myData;
            if (!isValid())
            {
                this.data = new int[9, 9];
            }
        }
        #endregion

        #region Contains functions
        /// <summary>
        /// Checks if the given row already contains the given number
        /// </summary>
        /// <param name="rowNumber">the row to check</param>
        /// <param name="contains">the number to look for</param>
        /// <returns>true if number was found, false otherwise</returns>
        public Boolean rowContains(int rowNumber, int contains)
        {
            for (int col=0; col<9; col++)
            {
                if (data[rowNumber,col] == contains)
                {
                    return true;
                }
            }
            return false;
        }

       /// <summary>
        /// Checks if the given column already contains the given number
        /// </summary>
        /// <param name="colNumber">the column to check</param>
        /// <param name="contains">the number to look for</param>
        /// <returns>true if number was found, false otherwise</returns>
        public Boolean colContains(int colNumber, int contains)
        {
            for (int row = 0; row < 9; row++)
            {
                if (data[row, colNumber] == contains)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the given square contains the given number
        /// </summary>
        /// <param name="square"> ID of the square (0 based, first row 0-2,
        /// second row 3-5, third row 6-8)</param>
        /// <param name="contains">the number to look for</param>
        /// <returns>true, if number was found, false otherwise</returns>
        public Boolean squareContains(int square, int contains)
        {
            int row = (square / 3) * 3;

            for (int i = 0; i < 2; i++)
            {
                int col = (square % 3) * 3;
                for (int j = 0; j < 2; j++)
                {
                    if(data[row,col] == contains)
                    {
                        return true;
                    }
                    col++;
                }
                row++;
            }
            return false;
        }
        #endregion

        #region getter and setter
        public int[,] getData()
        {
            return data;
        }

        public Boolean setData(int[,] newData)
        {
            int[,] oldData = data;
            data = newData;
            if (!isValid())
            {
                data = oldData;
                return false;
            }
            return true;
        }
        #endregion

        #region Others
        /// <summary>
        /// Checks if the saved grid is valid
        /// </summary>
        /// <returns>true, if valid, false otherwise</returns>
        public Boolean isValid()
        {

            #region check each row and column
            for (int i = 0; i < 9; i++)
            {
                List<int> colValues = new List<int>();
                List<int> rowValues = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    int row = data[i, j];
                    int col = data[j, i];

                    if (col != 0 && colValues.Contains(col))
                    {
                        MessageBox.Show(String.Format("Row {0} (column {1}) contains a duplicate number ({2}) for that column!", j, i, col));
                        return false;
                    }

                    if (row != 0 && rowValues.Contains(row))
                    {
                        MessageBox.Show(String.Format("Row {0} (column {1}) contains a duplicate number ({2}) for that row!", i, j, row));
                        return false;
                    }

                    colValues.Add(col);
                    rowValues.Add(row);
                }
            }
            #endregion

            #region check each square
            for (int square = 0; square < 9; square++)
            {
                List<int> squareValues = new List<int>();
                int rowStart = (square / 3) * 3;
                int rowMax = rowStart + 2;
                int colStart = (square % 3) * 3;
                int colMax = colStart + 2;

                for (int row = rowStart; row < rowMax; row++)
                {
                    for (int col = colStart; col < colMax; col++)
                    {
                        int current = data[row, col];

                        if (current != 0 && squareValues.Contains(current))
                        {
                            MessageBox.Show(String.Format("Square {0} contains a duplicate {1}", square, current));
                            return false;
                        }
                        squareValues.Add(current);
                    }
                }
            }
            #endregion

            return true;
        }
        #endregion
    }
}