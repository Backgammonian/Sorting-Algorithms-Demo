using System.Drawing;

namespace SortingAlgorithmsDemo
{
    public struct SortingUnit
    {
        public SortingUnit(double value, Color color, Color graduatedGrayColor, int initialPosition)
        {
            Value = value;
            Color = color;
            GraduatedGrayColor = graduatedGrayColor;
            InitialPosition = initialPosition;
        }

        public double Value { get; private set; }
        public Color Color { get; private set; }
        public Color GraduatedGrayColor { get; private set; }
        public int InitialPosition { get; private set; }
    }
}
