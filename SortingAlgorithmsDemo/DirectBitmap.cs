using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SortingAlgorithmsDemo
{
    public class DirectBitmap
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = x + (y * Width);
            int col = color.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Clear(Color color)
        {
            using (var gr = Graphics.FromImage(Bitmap))
            {
                gr.Clear(color);
            }
        }

        private bool Contains(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void DrawPoint(int x, int y, Color color)
        {
            if (Contains(x, y))
            {
                SetPixel(x, y, color);
            }
        }

        public void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                DrawPoint(x0, y0, color);

                if ((x0 == x1) && (y0 == y1))
                {
                    break;
                }

                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void FillRectangle(int x, int y, int w, int h, Color color)
        {
            for (var i = x; i <= x + w; i++)
            {
                DrawLine(i, y, i, y + h, color);
            }
        }

        private static Color Blend(Color color, Color backColor, double amount)
        {
            byte a = (byte)((color.A * amount) + backColor.A * (1 - amount));
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(a, r, g, b);
        }

        public void BlendPoint(int x, int y, Color newColor)
        {
            if (Contains(x, y))
            {
                SetPixel(x, y, Blend(newColor, GetPixel(x, y), 0.5));
            }
        }

        public void BlendLine(int x0, int y0, int x1, int y1, Color newColor)
        {
            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                BlendPoint(x0, y0, newColor);

                if ((x0 == x1) && (y0 == y1))
                {
                    break;
                }

                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void BlendRectangle(int x, int y, int w, int h, Color newColor)
        {
            for (var i = x; i <= x + w; i++)
            {
                BlendLine(i, y, i, y + h, newColor);
            }
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }

        public DirectBitmap Clone()
        {
            var bm = new DirectBitmap(Width, Height);
            using (var gr = Graphics.FromImage(bm.Bitmap))
            {
                gr.DrawImageUnscaled(Bitmap, 0, 0, Width, Height);
            }

            return bm;
        }
    }
}
