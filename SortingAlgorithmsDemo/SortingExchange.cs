namespace SortingAlgorithmsDemo
{
    public struct SortingExchange
    {
        public SortingExchange(int firstIndex, int secondIndex)
        {
            FirstIndex = firstIndex;
            SecondIndex = secondIndex;
        }

        public int FirstIndex { get; private set; }
        public int SecondIndex { get; private set; }
    }
}
