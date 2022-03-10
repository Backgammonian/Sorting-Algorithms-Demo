using System;
using System.Drawing;

namespace SortingAlgorithmsDemo
{
    public static class Utils
    {
        private static readonly Random _r = new Random();

        public static int Next(int minValue, int maxValue)
        {
            return _r.Next(minValue, maxValue);
        }

        public static Color GetRandomColor()
        {
            return Color.FromArgb((byte)_r.Next(0, 256), (byte)_r.Next(0, 256), (byte)_r.Next(0, 256));
        }
    }
}
