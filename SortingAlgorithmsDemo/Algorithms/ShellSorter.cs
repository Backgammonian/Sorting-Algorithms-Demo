using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class ShellSorter
    {
        public static void ShellSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            bool flag = true;
            int size = collection.Count;
            int d = collection.Count;
            while (flag || (d > 1))
            {
                flag = false;
                d = (d + 1) / 2;
                for (int i = 0; i < (size - d); i++)
                {
                    if (collection[i + d].Value < collection[i].Value)
                    {
                        swapFunction(i + d, i);

                        flag = true;
                    }
                }
            }
        }
    }
}
