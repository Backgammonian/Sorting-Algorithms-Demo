using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class CountingSorter
    {
        public static void CountingSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction)
        {
            int max = collection.MaxValue();
            int min = collection.MinValue();
            int range = max - min + 1;
            int[] count = new int[range];
            SortingUnit[] output = new SortingUnit[collection.Count];

            for (int i = 0; i < collection.Count; i++)
            {
                count[collection[i].Value - min]++;
            }

            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                output[count[collection[i].Value - min] - 1] = collection[i];
                count[collection[i].Value - min]--;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                placementFunction(i, output[i]);
            }
        }
    }
}
