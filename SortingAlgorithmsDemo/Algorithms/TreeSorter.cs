using System;
using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public static class TreeSorter
    {
        public static void TreeSort(this List<SortingUnit> collection, Action<int, SortingUnit> placementFunction)
        {
            var treeNode = new TreeNode(collection[0]);

            for (int i = 1; i < collection.Count; i++)
            {
                treeNode.Insert(new TreeNode(collection[i]));
            }

            var sorted = treeNode.Transform();
            for (int i = 0; i < sorted.Length; i++)
            {
                placementFunction(i, sorted[i]);
            }
        }
    }
}
