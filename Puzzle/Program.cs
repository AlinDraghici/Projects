using System;
using System.Collections.Generic;

public class Solution
{
    public static int ShortestPathToCollectKeys(string[] grid)
    {
        if (grid == null || grid.Length == 0)
            return -1;

        int m = grid.Length;
        int n = grid[0].Length;
        int keysCount = 0;
        int targetKeys = 0;

       
        int startX = 0;
        int startY = 0;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (grid[i][j] == '@')
                {
                    startX = i;
                    startY = j;
                }
                else if (Char.IsLower(grid[i][j]))
                {
                    keysCount++;
                    targetKeys |= (1 << (grid[i][j] - 'a')); 
                }
            }
        }

        int[][] directions = new int[][] {
            new int[] { -1, 0 }, 
            new int[] { 1, 0 },  
            new int[] { 0, -1 }, 
            new int[] { 0, 1 }  
        };

        Queue<int[]> queue = new Queue<int[]>();
        queue.Enqueue(new int[] { startX, startY, 0, 0 });

        HashSet<string> visited = new HashSet<string>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            int x = node[0];
            int y = node[1];
            int keysMask = node[2];
            int steps = node[3];

            if (keysMask == targetKeys)
                return steps;

            string key = x + "-" + y + "-" + keysMask;

            if (!visited.Contains(key))
            {
                visited.Add(key);

                for (int i = 0; i < 4; i++)
                {
                    int newX = x + directions[i][0];
                    int newY = y + directions[i][1];

                    if (newX >= 0 && newX < m && newY >= 0 && newY < n && grid[newX][newY] != '#')
                    {
                        char currentChar = grid[newX][newY];

                        if (Char.IsUpper(currentChar) && (keysMask & (1 << (currentChar - 'A'))) == 0)
                            continue;

                        int newKeysMask = keysMask;
                        if (Char.IsLower(currentChar))
                            newKeysMask |= (1 << (currentChar - 'a'));

                        queue.Enqueue(new int[] { newX, newY, newKeysMask, steps + 1 });
                    }
                }
            }
        }

        return -1;
    }

    public static void Main()
    {
        List<String> grid = new List<String>();
        String input = Console.ReadLine();
        input = input.Substring(1,input.Length - 2);

        foreach (var item in input.Replace("\"", "").Split(','))
        {
            grid.Add(item);
        }

        Console.WriteLine(ShortestPathToCollectKeys(grid.ToArray()));
    }
}