using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    //source: https://panthema.net/2013/sound-of-sorting/sound-of-sorting-0.6.5/src/SortAlgo.cpp.html

    public static class BitonicSorter
    {
        public static void BitonicSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            collection.BitonicSortInternal(0, collection.Count, true, swapFunction);
        }

        private static void BitonicSortInternal(this List<SortingUnit> collection, int lo, int n, bool dir, Action<int, int> swapFunction)
        {
            if (n > 1)
            {
                int m = n / 2;
                collection.BitonicSortInternal(lo, m, !dir, swapFunction);
                collection.BitonicSortInternal(lo + m, n - m, dir, swapFunction);
                collection.BitonicMerge(lo, n, dir, swapFunction);
            }
        }

        private static void BitonicMerge(this List<SortingUnit> collection, int lo, int n, bool dir, Action<int, int> swapFunction)
        {
            if (n > 1)
            {
                int m = GreatestPowerOfTwoLessThan(n);

                for (int i = lo; i < lo + n - m; i++)
                {
                    if (dir == (collection[i].Value > collection[i + m].Value))
                    {
                        swapFunction(i, i + m);
                    }
                }

                collection.BitonicMerge(lo, m, dir, swapFunction);
                collection.BitonicMerge(lo + m, n - m, dir, swapFunction);
            }
        }

        private static int GreatestPowerOfTwoLessThan(int n)
        {
            int k = 1;
            while (k < n)
            {
                k <<= 1;
            }

            return k >> 1;
        }
    }
}
