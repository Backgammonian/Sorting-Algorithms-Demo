using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class Functions
    {
        public static int IndexOfMax(this List<SortingUnit> collection)
        {
            int result = 0;
            int n = collection.Count - 1;
            for (int i = 1; i <= n; ++i)
            {
                if (collection[i].Value > collection[result].Value)
                {
                    result = i;
                }
            }

            return result;
        }

        public static int IndexOfMax(this List<SortingUnit> collection, int n)
        {
            int result = 0;
            for (int i = 1; i <= n; ++i)
            {
                if (collection[i].Value > collection[result].Value)
                {
                    result = i;
                }
            }

            return result;
        }

        public static int IndexOfMin(this List<SortingUnit> collection)
        {
            int result = 0;
            int n = collection.Count - 1;
            for (int i = 1; i <= n; ++i)
            {
                if (collection[i].Value < collection[result].Value)
                {
                    result = i;
                }
            }

            return result;
        }

        public static int IndexOfMin(this List<SortingUnit> collection, int n)
        {
            int result = 0;
            for (int i = 1; i <= n; ++i)
            {
                if (collection[i].Value < collection[result].Value)
                {
                    result = i;
                }
            }

            return result;
        }

        public static double MaxValue(this List<SortingUnit> collection)
        {
            double result = collection[0].Value;
            for (int i = 1; i < collection.Count; i++)
            {
                if (result < collection[i].Value)
                {
                    result = collection[i].Value;
                }
            }

            return result;
        }

        public static double MinValue(this List<SortingUnit> collection)
        {
            double result = collection[0].Value;
            for (int i = 1; i < collection.Count; i++)
            {
                if (result > collection[i].Value)
                {
                    result = collection[i].Value;
                }
            }

            return result;
        }
    }
}
