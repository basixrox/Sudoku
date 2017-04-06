using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solver
    {
        public int[,] Puzzle { get; set; }
        public int[,] Solution { get; set; }
        private static readonly Random rng = new Random();

        public bool solvePuzzle()
        {
            if (Puzzle == null) return false;

            Solution = Puzzle;
            return Solve();
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
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

        private bool Solve()
        {
            //Suche das nächste zu besetzende Feld
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Solution[row, col] != 0) continue;

                    List<int> availableValues = GetAvailableValues(row, col);

                    do
                    {
                        /* Abbruchbedingung 1 (false):
                         *  Es gibt keine verfügbaren Werte für das nächste zu besetzende Feld
                         *  Das Rätsel kann nicht gelöst werden -> Backtracking
                         */
                        if (availableValues.Count == 0)
                        {
                            Solution[row, col] = 0;
                            return false;
                        }

                        Solution[row, col] = availableValues[0];
                        availableValues.RemoveAt(0);
                    } while (this.Solve() == false); // Rekursiver Aufruf
                }
            }
            /* Abbruchbedingung 2 (true):
             *  Es sind alle Felder befüllt
             *  Das Rätsel wurde gelöst
             */
            return true;
        }

        private int DetermineSquare(int row, int col)
        {
            /*   0 │ 1 │ 2      row ist jeweils 0, 1 oder 2
             *  ───┼───┼───
             *   3 │ 4 │ 5      row ist jeweils 3, 4 oder 5
             *  ───┼───┼───
             *   6 │ 7 │ 8      row ist jeweils 6, 7 oder 8
             *   
             *   Berechnung:
             *   3 * (row / 3) ergibt 0, 3 oder 6
             *   addiert man (col / 3) dazu (0, 1 oder 2) ist das Ergebnis das korrekte Quadrat
             */

            return 3 * (row / 3) + col / 3;
        }
    }
}
