using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class InsertionSorter
    {
        public static void InsertionSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int size = collection.Count;
            for (int i = 1; i < size; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (collection[j].Value < collection[j - 1].Value)
                    {
                        swapFunction(j - 1, j);
                    }
                }
            }
        }
    }
}
