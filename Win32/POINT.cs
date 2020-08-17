using System.Runtime.InteropServices;

namespace PhoneRecognizer.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    // https://www.pinvoke.net/default.aspx/Structures/POINT.html
    public struct POINT
    {
        public readonly int X;
        public readonly int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }
}