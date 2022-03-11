namespace SortingAlgorithmsDemo
{
    public struct SortingPlacement
    {
        public SortingPlacement(int position, SortingUnit value)
        {
            Position = position;
            Value = value;
        }

        public int Position { get; private set; }
        public SortingUnit Value { get; private set; }
    }
}
