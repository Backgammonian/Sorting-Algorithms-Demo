using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class CycleSorter
    {
        public static void CycleSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction)
        {
            int n = collection.Count;
            for (int cycleStart = 0; cycleStart <= n - 2; cycleStart++)
            {
                SortingUnit item = collection[cycleStart];

                int position = cycleStart;
                for (int i = cycleStart + 1; i < n; i++)
                {
                    if (collection[i].Value < item.Value)
                    {
                        position++;
                    }
                }

                if (position == cycleStart)
                {
                    continue;
                }

                while (item.Value == collection[position].Value)
                {
                    position += 1;
                }

                if (position != cycleStart)
                {
                    SortingUnit temp = item;
                    item = collection[position];
                    //collection[position] = temp;

                    placementFunction(position, temp);
                }

                while (position != cycleStart)
                {
                    position = cycleStart;

                    for (int i = cycleStart + 1; i < n; i++)
                    {
                        if (collection[i].Value < item.Value)
                        {
                            position += 1;
                        }
                    }

                    while (item.Value == collection[position].Value)
                    {
                        position += 1;
                    }

                    if (item.Value != collection[position].Value)
                    {
                        SortingUnit temp = item;
                        item = collection[position];
                        //collection[position] = temp;

                        placementFunction(position, temp);
                    }
                }
            }
        }
    }
}
