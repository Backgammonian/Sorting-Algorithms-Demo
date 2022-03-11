using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class CombSorter
    {
        public static void CombSort(this List<SortingUnit> collection, Action<int, int> swapFunction)
        {
            int length = collection.Count;
            int currentStep = length - 1;

            while (currentStep > 1)
            {
                for (int i = 0; i + currentStep < collection.Count; i++)
                {
                    if (collection[i].Value > collection[i + currentStep].Value)
                    {
                        swapFunction(i, i + currentStep);
                    }
                }

                currentStep = GetNextStep(currentStep);
            }

            for (int i = 1; i < length; i++)
            {
                bool swapFlag = false;
                for (int j = 0; j < length - i; j++)
                {
                    if (collection[j].Value > collection[j + 1].Value)
                    {
                        swapFunction(j, j + 1);

                        swapFlag = true;
                    }
                }

                if (!swapFlag)
                {
                    break;
                }
            }
        }

        private static int GetNextStep(int s)
        {
            s = s * 1000 / 1247;
            return s > 1 ? s : 1;
        }
    }
}
