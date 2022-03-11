using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class SelectionSorter
    {
        public static void SelectionSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int size = collection.Count;
            int i;
            for (i = 0; i < size; i++)
            {
                int min = i;
                for (int j = i + 1; j < collection.Count; j++)
                {
                    if (collection[j].Value < collection[min].Value)
                    {
                        min = j;
                    }
                }

                swapFunction(i, min);
            }
        }
    }
}
