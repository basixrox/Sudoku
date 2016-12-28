using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SudokuGrid sudokuGrid;

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
            clearAllFields();
            initializeGlobals();
        }

        /// <summary>
        /// remove all values from all text inputs
        /// </summary>
        private void clearAllFields()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox current = (TextBox)this.FindName("txt" + row.ToString() + col.ToString());
                    current.Text = "";
                }
            }
        }

        /// <summary>
        /// resets all global variables
        /// </summary>
        private void initializeGlobals()
        {
            sudokuGrid = new SudokuGrid();
        }
        #endregion

        #region Buttons
        /// <summary>
        /// exits the program
        /// </summary>
        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// will be used to solve the entered sudoku puzzle
        /// </summary>
        private void solveButton(object sender, RoutedEventArgs e)
        {
            sudokuGrid.setData(createGridFromFields());
            if (sudokuGrid.isValid())
            {
                Solver solver = new Solver(sudokuGrid);
                sudokuGrid = solver.solve();
                if (!sudokuGrid.allFieldsFilled())
                {
                    MessageBox.Show("Some fields could not be filled - the puzzle is ambigious.", "Ambigious puzzle", MessageBoxButton.OK);
                }
                fillFieldsFromGrid(sudokuGrid.getData(), false);
            }
        }

        /// <summary>
        /// will be used to generate a new sudoku puzzle
        /// </summary>
        private void generateButton(object sender, RoutedEventArgs e)
        {
            //TODO
            MessageBoxResult generate = MessageBox.Show("Are you sure, you want to generate a random puzzle? All your changes will be lost.", "Please confirm", MessageBoxButton.YesNoCancel);

            if (generate == MessageBoxResult.Yes)
            {
                int[,] grid = new int[9, 9];
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        grid[i, j] = rnd.Next(1, 9);
                    }
                }
                fillFieldsFromGrid(grid, false);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Ask user to confirm before closing the program
        /// </summary>
        private void onClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult exit = MessageBox.Show("Are you sure, you want to exit this application? All your changes will be lost.", "Please confirm", MessageBoxButton.YesNoCancel);

            if (exit != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Make sure, that only numbers from 1-9 can be entered
        /// </summary>
        private void updatedGrid(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[^1-9].{0}");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// move to adjacent input fields with the arrow keys
        /// </summary>
        private void keyDown(object sender, KeyEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            int row = int.Parse(txtBox.Name.Substring(3, 1));
            int col = int.Parse(txtBox.Name.Substring(4, 1));

            switch (e.Key)
            {
                case Key.Down:
                    e.Handled = true;
                    row++;
                    break;
                case Key.Up:
                    e.Handled = true;
                    row--;
                    break;
                case Key.Left:
                    e.Handled = true;
                    col--;
                    break;
                case Key.Right:
                    e.Handled = true;
                    col++;
                    break;
            }

            if (row < 0 || row > 8 || col < 0 || col > 8)
            {
                return;
            }

            txtBox = (TextBox)this.FindName("txt" + row.ToString() + col.ToString());
            txtBox.Focus();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Extracts all numbers from the texboxes in the view and saves
        /// them in an array
        /// </summary>
        private int[,] createGridFromFields()
        {
            int[,] newGrid = new int[9, 9];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0;col <9; col++)
                {
                    TextBox txtBox = (TextBox)this.FindName("txt" + row.ToString() + col.ToString());
                    if(txtBox.Text != "")
                    {
                        newGrid[row, col] = int.Parse(txtBox.Text);
                    } else
                    {
                        newGrid[row, col] = 0;
                    }
                }
            }
            return newGrid;
        }

        /// <summary>
        /// publishes an int array [,] to the text boxes of the View
        /// </summary>
        /// <param name="grid">the array to show</param>
        private void fillFieldsFromGrid(int[,] grid, bool readOnly = false)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox txtBox = (TextBox)this.FindName("txt" + row.ToString() + col.ToString());
                    int number = grid[row, col];
                    if (number != 0)
                    {
                        txtBox.Text = number.ToString();
                        txtBox.IsReadOnly = readOnly;
                    }
                }
            }
        }
        #endregion
    }
}
