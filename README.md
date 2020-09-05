# Sudoku

SudokuLibrary To Make / Solve Sudoku.

NameSpace:
using SudokuLibrary;

Init:

int[,] myGrid = new int[9,9]
int[,] myGrid_16 = new int[16,16]

Sudoku sudoku = new Sudoku(); //default 9x9 Empty grid
Sudoku sudoku = new Sudoku(myGrid); //use of custom 9x9 myGrid
Sudoku sudoku = new Sudoku(16,4) //use of default 16x16 Empty grid
Sudoku sudoku = new Sudoku(16,4,myGrid_16) //use of custom 16x16 myGrid_16

APIs:

