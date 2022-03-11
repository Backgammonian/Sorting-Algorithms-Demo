using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class PancakeSorter
    {
        public static void PancakeSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int size = collection.Count;
            for (var subArrayLength = size - 1; subArrayLength >= 0; subArrayLength--)
            {
                var indexOfMax = collection.IndexOfMax(subArrayLength);
                if (indexOfMax != subArrayLength)
                {
                    Flip(indexOfMax, swapFunction);
                    Flip(subArrayLength, swapFunction);
                }
            }
        }

        private static void Flip(int end, Action<int, int> swapFunction)
        {
            for (int start = 0; start < end; start++, end--)
            {
                swapFunction(start, end);
            }
        }
    }
}
