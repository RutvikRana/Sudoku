using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// SudokuLibrary - To make/solve Sudokus
/// Practically Support 2x2,3x3,4x4 (5x5 maybe used)
/// Created By Rutvik H. Rana ( rutvikrana512@gmail.com )
/// </summary>
namespace SudokuLibrary
{
    /// <summary>
    ///  Sudoku Class To Make/Solve Custom Sudoku.
    /// </summary>
    public class Sudoku
    {
        // Grid[0,5] = 6 means ( 0 row, 5 column contains 6)
        /// <summary>
        /// Saved Array of given Grid (Default is Empty Grid)
        /// </summary>
        public int[,] Grid;
        private int[,] _Grid;
        private int _BoxNum = 3;
        private int _GridNum = 9;
        private int maxCount = 0;
        /// <summary>
        /// Max Cycles To Solve Sudoku Recursively (Increase It If You got Failed Message)
        /// (Default = 50000)
        /// </summary>
        public int MaxCycles = 50000;
        public void CopyArray(int[,] original_arr, int[,] copy_arr) {
            System.Array.Copy(original_arr, copy_arr, original_arr.Length);
        }

        /// <summary>
        /// Sudoku Constructor with default empty 9x9 grid
        /// </summary>
        public Sudoku()
        {
            Grid = new int[9, 9];
            _Grid = new int[9, 9];
        }
        /// <summary>
        /// Sudoku Constructor with custom 9x9 grid
        /// </summary>
        public Sudoku(int[,] grid) {
            Grid = new int[9, 9];
            _Grid = new int[9, 9];
            Grid = (int[,])grid.Clone();
            _Grid = (int[,])grid.Clone();
        }
        /// <summary>
        /// Sudoku Constructor with custom ( gridNum x gridNum ) grid with (boxNum x boxNum) Boxes
        /// Ex. Default ( 9x9 ) grid with (3x3) boxes
        /// </summary>
        public Sudoku(int gridNum,int boxNum, int[,] grid) {
            Grid = new int[gridNum, gridNum];
            _Grid = new int[gridNum, gridNum];
            Grid = (int[,])grid.Clone();
            _Grid = (int[,])grid.Clone();
            _BoxNum = boxNum;
            _GridNum = gridNum;
        }
        /// <summary>
        /// Sudoku Constructor with Empty ( gridNum x gridNum ) grid with (boxNum x boxNum) Boxes
        /// </summary>
        public Sudoku(int gridNum, int boxNum)
        {
            Grid = new int[gridNum, gridNum];
            _Grid = new int[gridNum, gridNum];
            _BoxNum = boxNum;
            _GridNum = gridNum;
        }
        private bool IsSafeToPlace(int a, int b, int num) {

            if (_Grid[a, b] != 0) { return false; }

            for (int i = 0; i < _GridNum; i++) {
                if (_Grid[a, i] == num) { return false; }
                if (_Grid[i, b] == num) { return false; }
            }

            int firstCell_row = (a / _BoxNum) * _BoxNum;
            int firstCell_col = (b / _BoxNum) * _BoxNum;
            for (int i = 0; i < _BoxNum; i++) {
                for (int j = 0; j < _BoxNum; j++)
                {
                    if (_Grid[firstCell_row + i, firstCell_col + j] == num) { return false; }
                }
            }

            return true;
        }

        public bool IsSafeToPlace(int[,] grid,int a, int b, int num)
        {

            if (grid[a, b] != 0) { return false; }

            for (int i = 0; i < _GridNum; i++)
            {
                if (grid[a, i] == num) { return false; }
                if (grid[i, b] == num) { return false; }
            }

            int firstCell_row = (a / _BoxNum) * _BoxNum;
            int firstCell_col = (b / _BoxNum) * _BoxNum;
            for (int i = 0; i < _BoxNum; i++)
            {
                for (int j = 0; j < _BoxNum; j++)
                {
                    if (grid[firstCell_row + i, firstCell_col + j] == num) { return false; }
                }
            }

            return true;
        }

        private (int,int) FindEmpty() {

            for (int i = 0; i < _GridNum; i++) {
                for (int j = 0; j < _GridNum; j++)
                {
                    if (_Grid[i, j] == 0) {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        public (int, int) FindEmpty(int[,] grid)
        {

            for (int i = 0; i < _GridNum; i++)
            {
                for (int j = 0; j < _GridNum; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        /// <summary>
        /// Is this.Grid is solved or not ?
        /// </summary>
        /// <returns></returns>
        public bool IsSolved() {
            return IsSolved(Grid);
        }
        /// <summary>
        /// Is given grid is solved or not ?
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool IsSolved(int[,] grid)
        {
            for (int i = 0; i < _GridNum; i++)
            {
                for (int j = 0; j < _GridNum; j++)
                {

                    if (i % _BoxNum == 0 && j % _BoxNum == 0)
                    {
                        for (int r = 0; r < _BoxNum; r++)
                        {
                            for (int c = 0; c < _BoxNum; c++)
                            {
                                for (int r2 = 0; r2 < _BoxNum; r2++)
                                {
                                    for (int c2 = 0; c2 < _BoxNum; c2++)
                                    {
                                        if (r == r2 && c == c2) { continue; }

                                        if (grid[i + r, j + c] == grid[i + r2, j + c2]) { return false; }
                                    }
                                }

                            }
                        }
                    }

                    for (int k = 0; k < _GridNum; k++)
                    {
                        if (j == k) { continue; }
                        if (grid[i, j] == grid[i, k]) { return false; }
                    }
                }
            }

            return true;

        }

        private bool _LinearSolve() {
            (int a, int b) = FindEmpty();
            maxCount++;
            if (maxCount > MaxCycles)
            {
                Debug.Log("Failed To Solve :: Cycles Exceeds " + MaxCycles);
                maxCount = 0;
                return true;
            }

            if (a == -1) {
                return true;
            }

            for (int i = 1; i <= _GridNum; i++) {
                if (IsSafeToPlace(a, b, i)) {
                    _Grid[a, b] = i;

                    if (_LinearSolve())
                    {
                        return true;
                    }
                    else {
                        _Grid[a, b] = 0;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// return Solved Sudoku Grid
        /// </summary>
        /// <returns></returns>
        public int[,] LinearSolve() {
            maxCount = 0;
            _Grid = (int[,])Grid.Clone();
            if (_LinearSolve())
            {
                return _Grid;
            }
            else {
                return null;
            }
        }

        private (int,int,int[]) GuessFill(int[,] grid) {

            int row = -1;
            int col = -1;
            List<int> myList = new List<int>();
            bool loop = true;

            while (loop) {

                row = -1;
                col = -1;
                loop = false;
                myList.Clear();

                for (int i = 0; i < _GridNum; i++) {
                    for (int j = 0; j < _GridNum; j++)
                    {
                        if (grid[i, j] == 0)
                        {
                            List<int> l = new List<int>();
                            for (int k = 1; k <= _GridNum; k++)
                            {
                                if (IsSafeToPlace(grid, i, j, k))
                                {
                                    l.Add(k);
                                }
                            }

                            if (l.Count == 0) { return (-1, -1, null); }
                            else if (l.Count == 1) { grid[i, j] = l[0]; loop = true; }
                            else if (l.Count > 1)
                            {
                                if (row == -1) {
                                    row = i;
                                    col = j;
                                    myList.Clear();
                                    myList.AddRange(l);
                                }

                                if (myList.Count > l.Count) {
                                    row = i;
                                    col = j;
                                    myList.Clear();
                                    myList.AddRange(l);
                                }
                            }
                        }
                    }

                }

                if (!loop && row == -1) {
                    return (-2, -2, null);
                }
            }

            return (row, col, myList.ToArray());
        }

        private bool _Solve(int[,] grid) {
            maxCount++;
            if (maxCount > MaxCycles) {
                Debug.Log("Failed To Solve :: Cycles Exceeds MaxCycles " + MaxCycles);
                _Grid = null;
                return true;
            }
            int[,] newGrid = (int[,])grid.Clone();
            (int row, int col, int[] guesses) = GuessFill(newGrid);

            if (row == -2)
            {
                _Grid = (int[,])newGrid.Clone();
                return true;
            }
            else if (row == -1)
            {
                return false;
            }

            else {
                for (int i = 0; i < guesses.Length; i++) {
                    newGrid[row, col] = guesses[i];
                    if (_Solve(newGrid))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// return Solved Sudoku Grid
        /// </summary>
        /// <returns></returns>
        public int[,] Solve() {
            maxCount = 0;
            _Grid = (int[,])Grid.Clone();
            if (_Solve(_Grid))
            {
                return _Grid;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Make New Sudoku Grid
        /// </summary>
        /// <returns></returns>
        public int[,] Make()
        {
            _Grid = new int[_GridNum, _GridNum];
            for (int i = 0; i < _GridNum / _BoxNum; i++) {
                int row = i*_BoxNum;
                int[] nums = new int[_GridNum];
                nums = RandomMethods.UniqueRange(1, _GridNum+1);
                for (int j = 0; j < _BoxNum; j++)
                {
                    for (int k = 0; k < _BoxNum; k++)
                    {
                        _Grid[row + j, row + k] = nums[j * _BoxNum + k];
                    }
                }

            }

            if (_Solve(_Grid))
            {
                return _Grid;
            }
            else
            {
                return null;
            }
;
        }
        /// <summary>
        /// Make New Sudoku Grid with K amount of empty places
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public int[,] Make(int K)
        {
            _Grid = Make();
            if (_Grid != null)
            {
                (int a, int b) = FindEmpty(_Grid);

                if(a == -1)
                {
                    return RemoveKPlaces(K, _Grid);
                }
            }

            return null;
        }
        /// <summary>
        /// return ( SolvedGrid , NewSudokuGrid )
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public (int[,], int[,]) SolveMake(int K) {
            _Grid = Make();
            if (_Grid != null)
            {
                (int a, int b) = FindEmpty(_Grid);

                if (a == -1)
                {
                    return ((int[,])_Grid.Clone(),RemoveKPlaces(K, _Grid));
                }
            }

            return (null,null);
        }

        /// <summary>
        /// Make New Sudoku Grid from this.Grid with K amount of empty places
        /// (same as Solve Grid then remove K places)
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public int[,] MakeFromGrid(int K) {
            _Grid = Solve();
            if (_Grid != null)
            {
                (int a, int b) = FindEmpty(_Grid);

                if (a == -1)
                {
                    return RemoveKPlaces(K, _Grid);
                }
            }

            return null;
        }
        /// <summary>
        /// Remove K amount of places from grid.
        /// </summary>
        /// <param name="K"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public int[,] RemoveKPlaces(int K,int[,] grid) {

            if (grid == null) { return null; }

            int[] nums = new int[_GridNum * _GridNum];
            nums = RandomMethods.UniqueRange(0, nums.Length);

            for (int i = 0; i < K; i++)
            {
                int row = nums[i] / _GridNum;
                int col = nums[i] % _GridNum;
                grid[row, col] = 0;
            }

            return grid;

        }

        public void DebugShow(int[,] grid) {

            if (grid == null) {
                Debug.Log("Given Grid is Null");
                return;
            }

            string s = "";
            for (int i = 0; i < _GridNum; i++)
            {
                for (int j = 0; j < _GridNum; j++)
                {
                    try
                    {
                        s += grid[i, j] + ",";
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                s += "\n";
            }
            Debug.Log(s);
        }
    }

    public abstract class RandomMethods {
        /// <summary>
        /// Make Unique Random Numbers Array
        /// (min - inclusive , max - exclusive)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int[] UniqueRange(int min, int max) {

            if (min >= max) {
                return null;
            }

            int[] num = new int[max-min];
            for (int i = 0; i < num.Length; i++) {
                num[i] = min + i;
            }
            Shuffle(num);
            return num;
        }

        /// <summary>
        ///   Shuffle The Given Array
        /// </summary>
        /// <param name="arr"></param>

        public static void Shuffle(int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                int rand = Random.Range(0, arr.Length);
                int temp = arr[i];
                arr[i] = arr[rand];
                arr[rand] = temp;
            }
        }
    }
}
