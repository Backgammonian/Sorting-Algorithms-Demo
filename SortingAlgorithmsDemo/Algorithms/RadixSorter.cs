using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class RadixSorter
    {
        public static void RadixSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction)
        {
            int m = collection.MaxValue();

            for (int exp = 1; m / exp > 0; exp *= 10)
            {
                collection.AuxiliaryCountSort(placementFunction, exp);
            }
        }

        private static void AuxiliaryCountSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction, int exp)
        {
            int i;
            int[] count = new int[10];
            SortingUnit[] output = new SortingUnit[collection.Count];

            for (i = 0; i < count.Length; i++)
            {
                count[i] = 0;
            }

            for (i = 0; i < collection.Count; i++)
            {
                count[collection[i].Value / exp % 10]++;
            }

            for (i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            for (i = collection.Count - 1; i >= 0; i--)
            {
                output[count[collection[i].Value / exp % 10] - 1] = collection[i];
                count[collection[i].Value / exp % 10]--;
            }

            for (i = 0; i < collection.Count; i++)
            {
                placementFunction(i, output[i]);
            }
        }
    }
}
