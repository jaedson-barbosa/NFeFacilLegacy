using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OptimizedZXing
{
    public class BarcodeWriter
    {
        public BarcodeFormat Format { get; }
        public EncodingOptions Options { get; set; }
        public IWriter Encoder { get; set; }

        public BarcodeWriter(BarcodeFormat format)
        {
            Format = format;
            Options = new EncodingOptions { Height = 100, Width = 100 };
            switch (format)
            {
                case BarcodeFormat.CODE_128:
                    Encoder = new OneD.Code128Writer();
                    break;
                case BarcodeFormat.EAN_8:
                    Encoder = new OneD.EAN8Writer();
                    break;
                case BarcodeFormat.EAN_13:
                    Encoder = new OneD.EAN13Writer();
                    break;
                case BarcodeFormat.QR_CODE:
                    Encoder = new QR.QRCodeWriter();
                    break;
            }
        }

        public Rectangle[] WriteToUI(BitMatrix matrix)
        {
            return Renderer.RenderUI(matrix, Options);
        }

        public BitMatrix Encode(string contents)
        {
            var encoder = Encoder;
            var currentOptions = Options;
            return encoder.Encode(contents, Format, currentOptions.Width, currentOptions.Height, currentOptions.Margin);
        }

        public ImageSource WriteToBitmap(BitMatrix encoded)
        {
            return Renderer.RenderBitmap(encoded, Options);
        }
    }
}
