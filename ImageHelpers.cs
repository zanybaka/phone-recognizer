using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PhoneRecognizer.Win32;
using Size = System.Drawing.Size;

namespace PhoneRecognizer
{
    public static class ImageHelpers
    {
        // https://stackoverflow.com/questions/3072349/capture-screenshot-including-semitransparent-windows-in-net/3072580#3072580
        public static Bitmap CaptureBitmap(POINT point, Size sz)
        {
            IntPtr hDesk = NativeMethods.GetDesktopWindow();
            IntPtr hSrc = NativeMethods.GetWindowDC(hDesk);
            IntPtr hDest = NativeMethods.CreateCompatibleDC(hSrc);
            IntPtr hBmp = NativeMethods.CreateCompatibleBitmap(hSrc, sz.Width, sz.Height);
            IntPtr hOldBmp = NativeMethods.SelectObject(hDest, hBmp);
            bool res = NativeMethods.BitBlt(hDest, 0, 0, sz.Width, sz.Height, hSrc, point.X - sz.Width / 2, point.Y - sz.Height / 2, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
            if (!res)
            {
                return null;
            }
            Bitmap bmp = Image.FromHbitmap(hBmp);
            NativeMethods.SelectObject(hDest, hOldBmp);
            NativeMethods.DeleteObject(hBmp);
            NativeMethods.DeleteDC(hDest);
            NativeMethods.ReleaseDC(hDesk, hSrc);
            return bmp;
        }

        public static ImageSource GetImageSource(Bitmap bmp)
        {
            var img = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return img;
        }

        public static byte[] ImageToByte(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
    }
}
