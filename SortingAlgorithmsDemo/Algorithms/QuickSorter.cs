using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class QuickSorter
    {
        public static void QuickSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int startIndex = 0;
            int endIndex = collection.Count - 1;

            collection.InternalQuickSort(startIndex, endIndex, swapFunction);
        }

        private static void InternalQuickSort(this List<SortingUnit> collection, int leftmostIndex, int rightmostIndex, Action<int, int> swapFunction)
        {
            if (leftmostIndex < rightmostIndex)
            {
                int wallIndex = collection.InternalPartition(leftmostIndex, rightmostIndex, swapFunction);
                collection.InternalQuickSort(leftmostIndex, wallIndex - 1, swapFunction);
                collection.InternalQuickSort(wallIndex + 1, rightmostIndex, swapFunction);
            }
        }

        private static int InternalPartition(this List<SortingUnit> collection, int leftmostIndex, int rightmostIndex, Action<int, int> swapFunction)
        {
            int wallIndex, pivotIndex;

            pivotIndex = rightmostIndex;
            var pivotValue = collection[pivotIndex];

            wallIndex = leftmostIndex;

            for (int i = leftmostIndex; i <= (rightmostIndex - 1); i++)
            {
                if (collection[i].Value <= pivotValue.Value)
                {
                    swapFunction(i, wallIndex);

                    wallIndex++;
                }
            }

            swapFunction(wallIndex, pivotIndex);

            return wallIndex;
        }
    }
}
