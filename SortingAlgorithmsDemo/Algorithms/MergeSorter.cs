using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class MergeSorter
    {
        private static void Merge(List<SortingUnit> collection, Action<int, SortingUnit> placementFunction, int lowIndex, int middleIndex, int highIndex)
        {
            int left = lowIndex;
            int right = middleIndex + 1;
            SortingUnit[] tempArray = new SortingUnit[highIndex - lowIndex + 1];
            int index = 0;

            while ((left <= middleIndex) && (right <= highIndex))
            {
                if (collection[left].Value < collection[right].Value)
                {
                    tempArray[index] = collection[left];
                    left++;
                }
                else
                {
                    tempArray[index] = collection[right];
                    right++;
                }

                index++;
            }

            for (int i = left; i <= middleIndex; i++)
            {
                tempArray[index] = collection[i];
                index++;
            }

            for (int i = right; i <= highIndex; i++)
            {
                tempArray[index] = collection[i];
                index++;
            }

            for (int i = 0; i < tempArray.Length; i++)
            {
                placementFunction(lowIndex + i, tempArray[i]);
            }
        }

        private static void InternalMergeSort(List<SortingUnit> collection, Action<int, SortingUnit> placementFunction, int lowIndex, int highIndex)
        {
            if (lowIndex < highIndex)
            {
                var middleIndex = (lowIndex + highIndex) / 2;
                InternalMergeSort(collection, placementFunction, lowIndex, middleIndex);
                InternalMergeSort(collection, placementFunction, middleIndex + 1, highIndex);
                Merge(collection, placementFunction, lowIndex, middleIndex, highIndex);
            }
        }

        public static void MergeSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction)
        {
            InternalMergeSort(collection, placementFunction, 0, collection.Count - 1);
        }
    }
}
