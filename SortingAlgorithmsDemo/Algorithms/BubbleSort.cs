using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class BubbleSort
    {
        public static void BubbleSortAscending(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int size = collection.Count;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = size - 1; j > i; j--)
                {
                    if (collection[j - 1].Value > collection[j].Value)
                    {
                        swapFunction(j - 1, j);
                    }
                }
            }
        }

        public static void BubbleSortDescending(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int size = collection.Count;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = size - 1; j > i; j--)
                {
                    if (collection[j - 1].Value < collection[j].Value)
                    {
                        swapFunction(j - 1, j);
                    }
                }
            }
        }
    }
}
