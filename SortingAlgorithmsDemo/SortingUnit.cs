using System.Drawing;

namespace SortingAlgorithmsDemo
{
    public struct SortingUnit
    {
        public SortingUnit(int value, Color color, Color graduatedGrayColor)
        {
            Value = value;
            Color = color;
            GraduatedGrayColor = graduatedGrayColor;
        }

        public int Value { get; private set; }
        public Color Color { get; private set; }
        public Color GraduatedGrayColor { get; private set; }
    }
}
