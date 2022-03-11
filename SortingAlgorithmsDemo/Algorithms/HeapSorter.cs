using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class HeapSorter
    {
        public static void HeapSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int n = collection.Count;
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                collection.Heapify(swapFunction, n, i);
            }

            for (int i = n - 1; i >= 0; i--)
            {
                swapFunction(0, i);

                collection.Heapify(swapFunction, i, 0);
            }
        }

        private static void Heapify(this List<SortingUnit> collection, Action<int, int> swapFunction, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && collection[left].Value > collection[largest].Value)
            {
                largest = left;
            }

            if (right < n && collection[right].Value > collection[largest].Value)
            {
                largest = right;
            }

            if (largest != i)
            {
                swapFunction(i, largest);

                collection.Heapify(swapFunction, n, largest);
            }
        }
    }
}
