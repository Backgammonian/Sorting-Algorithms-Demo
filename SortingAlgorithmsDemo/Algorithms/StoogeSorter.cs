using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class StoogeSorter
    {
        public static void StoogeSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int startIndex = 0;
            int endIndex = collection.Count - 1;

            collection.InternalStoogeSort(startIndex, endIndex, swapFunction);
        }

        private static void InternalStoogeSort(this List<SortingUnit> collection, int startIndex, int endIndex, Action<int, int> swapFunction)
        {
            if (collection[startIndex].Value > collection[endIndex].Value)
            {
                swapFunction(startIndex, endIndex);
            }

            if (endIndex - startIndex > 1)
            {
                var len = (endIndex - startIndex + 1) / 3;
                collection.InternalStoogeSort(startIndex, endIndex - len, swapFunction);
                collection.InternalStoogeSort(startIndex + len, endIndex, swapFunction);
                collection.InternalStoogeSort(startIndex, endIndex - len, swapFunction);
            }
        }
    }
}
