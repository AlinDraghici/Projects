using System;

public class SudokuSolver
{
    public static void SolveSudoku(List<List<int>> board, int n, int m)
    {
        if (board == null || n * m == 0)
            return;

        Solve(board, n, m);
    }

    private static bool Solve(List<List<int>> board, int n, int m)
    {
        int row = 0;
        int col = 0;
        bool isEmpty = true;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (board[i][j] == 0)
                {
                    row = i;
                    col = j;
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
                break;
        }

        if (isEmpty)
            return true;

        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(board, n, m, row, col, num))
            {
                board[row][col] = num;

                if (Solve(board, n, m))
                    return true;

                board[row][col] = 0; 
            }
        }

        return false; 
    }

    private static bool IsValid(List<List<int>> board, int n, int m, int row, int col, int num)
    {
        for (int j = 0; j < m; j++)
        {
            if (board[row][j] == num)
                return false;
        }

        for (int i = 0; i < n; i++)
        {
            if (board[i][col] == num)
                return false;
        }

        int subgridStartRow = (row / 3) * 3;
        int subgridStartCol = (col / 3) * 3;
        for (int i = subgridStartRow; i < subgridStartRow + 3; i++)
        {
            for (int j = subgridStartCol; j < subgridStartCol + 3; j++)
            {
                if (board[i][j] == num)
                    return false;
            }
        }

        return true; 
    }

    public static void PrintBoard(List<List<int>> board, int n, int m)
    {

        Console.Write("[");
        for (int i = 0; i < n; i++)
        {
            Console.Write("[");
            for (int j = 0; j < m; j++)
                Console.Write(String.Format("\"{0}\"{1}", board[i][j], j != m - 1 ? "," : ""));
            Console.Write(String.Format("]{0}", i != n - 1 ? ",\n" : ""));
        }
        Console.Write("]");
    }

    public static void Main()
    {
        int n = 0, m = 0;
        List<List<int>> board = new List<List<int>>();

        string input = Console.ReadLine();
        input = input.Substring(1, input.Length - 2).Replace("\"", "");


        foreach (var item in input.Split("],["))
        {
            board.Add(new List<int>());
            m = 0;
            foreach (var number in item.Trim('[').Trim(']').Split(','))
            {
                board[n].Add(number == "." ? 0 : Convert.ToInt32(number));
                m++;
            }
            n++;
        }

        SolveSudoku(board, n, m);
        PrintBoard(board, n, m);
    }
}

