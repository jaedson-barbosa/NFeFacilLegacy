using Windows.UI.Xaml.Media.Imaging;

namespace OptimizedZXing
{
    public class BarcodeWriter
    {
        WriteableBitmapRenderer Renderer { get; }
        public BarcodeFormat Format { get; }
        public EncodingOptions Options { get; set; }
        public IWriter Encoder { get; set; }

        public BarcodeWriter(BarcodeFormat format)
        {
            Renderer = new WriteableBitmapRenderer();
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

        /// <summary>
        /// Encodes the specified contents and returns a rendered instance of the barcode.
        /// For rendering the instance of the property Renderer is used and has to be set before
        /// calling that method.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <returns></returns>
        public WriteableBitmap Write(string contents)
        {
            var matrix = Encode(contents);
            return Renderer.Render(matrix, Format, contents, Options);
        }

        /// <summary>
        /// Returns a rendered instance of the barcode which is given by a BitMatrix.
        /// For rendering the instance of the property Renderer is used and has to be set before
        /// calling that method.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        public WriteableBitmap Write(BitMatrix matrix)
        {
            return Renderer.Render(matrix, Format, null, Options);
        }

        public BitMatrix Encode(string contents)
        {
            var encoder = Encoder;
            var currentOptions = Options;
            return encoder.Encode(contents, Format, currentOptions.Width, currentOptions.Height, currentOptions.Margin);
        }
    }
}
