using System.Drawing;
using System.Windows;
using System.Windows.Media;
using PhoneRecognizer.OCR;
using PhoneRecognizer.Win32;
using Size = System.Drawing.Size;

namespace PhoneRecognizer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!NativeMethods.GetCursorPos(out POINT point))
            {
                return;
            }
            using (Bitmap bmp = ImageHelpers.CaptureBitmap(point, new Size(300, 50)))
            {
                ImageSource img = ImageHelpers.GetImageSource(bmp);
                imgControl.Width = img.Width;
                imgControl.Height = img.Height;
                imgControl.Source = img;
                recognizedText.Text = TesseractOCR.RecognizeText(ImageHelpers.ImageToByte(bmp));
                recognizedBlocks.Text = TesseractOCR.RecognizeBlocks(ImageHelpers.ImageToByte(bmp));
            }
        }
    }
}
