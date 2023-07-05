// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;

int n = 0, m = 0;
List<List<int>> array = new List<List<int>>();

string input = Console.ReadLine();
input = input.Substring(1, input.Length - 2);


foreach (var item in input.Split("],["))
{
    array.Add(new List<int>());
    m = 0;
    foreach (var number in item.Trim('[').Trim(']').Split(','))
    {
        array[n].Add(Convert.ToInt32(number));
        m++;
    }
    n++;
}

for(int i = 0; i < n; i++)
    for (int j = i; j < m; j++)
    {
        int temp = array[i][j];
        array[i][j] = array[j][i];
        array[j][i] = temp;
    }

for (int i = 0; i < n; i++)
    for (int j = 0; j < m / 2; j++)
    {
        int temp = array[i][j];
        array[i][j] = array[i][m - 1 - j];
        array[i][m - 1 - j] = temp;
    }

Console.Write("[");
for (int i = 0; i < n; i++)
{
    Console.Write("[");
    for (int j = 0; j < m; j++)
        Console.Write(String.Format("{0}{1}",array[i][j], j != m - 1 ? ",":""));
    Console.Write(String.Format("]{0}", i != n - 1 ? "," : ""));
}
Console.Write("]");