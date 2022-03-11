using System.Collections.Generic;

namespace SortingAlgorithmsDemo.Algorithms
{
    public class TreeNode
    {
        public TreeNode(SortingUnit data)
        {
            Data = data;
        }

        public SortingUnit Data { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public void Insert(TreeNode node)
        {
            if (node.Data.Value < Data.Value)
            {
                if (Left == null)
                {
                    Left = node;
                }
                else
                {
                    Left.Insert(node);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = node;
                }
                else
                {
                    Right.Insert(node);
                }
            }
        }

        public SortingUnit[] Transform(List<SortingUnit> elements = null)
        {
            if (elements == null)
            {
                elements = new List<SortingUnit>();
            }

            if (Left != null)
            {
                Left.Transform(elements);
            }

            elements.Add(Data);

            if (Right != null)
            {
                Right.Transform(elements);
            }

            return elements.ToArray();
        }
    }
}
