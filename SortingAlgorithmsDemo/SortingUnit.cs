using System.Drawing;

namespace SortingAlgorithmsDemo
{
    public class SortingUnit
    {
        public SortingUnit(int value, Color color, Color graduatedGrayColor)
        {
            Value = value;
            Color = color;
            GraduatedGrayColor = graduatedGrayColor;
        }

        public SortingUnit(int value, Color color)
        {
            Value = value;
            Color = color;
            GraduatedGrayColor = Color.White;
        }

        public int Value { get; private set; }
        public Color Color { get; private set; }
        public Color GraduatedGrayColor { get; private set; }

        public void SetGraduatedGrayColor(Color grayColor)
        {
            GraduatedGrayColor = grayColor;
        }
    }
}
