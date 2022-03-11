using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class ShakerSorter
    {
        public static void ShakerSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int left = 0;
            int right = collection.Count - 1;
            bool flag = true;

            while ((left < right) && flag)
            {
                flag = false;
                for (int i = left; i < right; i++)
                {
                    if (collection[i].Value > collection[i + 1].Value)
                    {
                        swapFunction(i, i + 1);

                        flag = true;
                    }
                }

                right--;

                for (int i = right; i > left; i--)
                {
                    if (collection[i - 1].Value > collection[i].Value)
                    {
                        swapFunction(i, i - 1);

                        flag = true;
                    }
                }

                left++;
            }
        }
    }
}
