using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class GnomeSorter
    {
        public static void GnomeSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int index = 1;
            int nextIndex = index + 1;

            while (index < collection.Count)
            {
                if (collection[index - 1].Value < collection[index].Value)
                {
                    index = nextIndex;
                    nextIndex++;
                }
                else
                {
                    swapFunction(index - 1, index);

                    index--;

                    if (index == 0)
                    {
                        index = nextIndex;
                        nextIndex++;
                    }
                }
            }
        }
    }
}
