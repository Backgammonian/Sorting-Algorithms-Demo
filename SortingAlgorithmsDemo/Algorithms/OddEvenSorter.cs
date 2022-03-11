using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class OddEvenSorter
    {
        public static void OddEvenSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 1; i < collection.Count - 1; i += 2)
                {
                    if (collection[i].Value > collection[i + 1].Value)
                    {
                        swapFunction(i, i + 1);

                        sorted = false;
                    }
                }

                for (int i = 0; i < collection.Count - 1; i += 2)
                {
                    if (collection[i].Value > collection[i + 1].Value)
                    {
                        swapFunction(i, i + 1);

                        sorted = false;
                    }
                }
            }
        }
    }
}
