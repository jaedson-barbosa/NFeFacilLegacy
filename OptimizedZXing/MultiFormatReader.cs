using OptimizedZXing.QR;

namespace OptimizedZXing
{
    internal class MultiFormatReader
    {
        private IReader Reader;

        public MultiFormatReader(BarcodeFormat format)
        {
            if (format == BarcodeFormat.EAN_13 ||
                format == BarcodeFormat.EAN_8 ||
                format == BarcodeFormat.CODE_128)
            {
                Reader = new MultiFormatOneDReader(format);
            }
            else if (format == BarcodeFormat.QR_CODE)
            {
                Reader = new QRCodeReader();
            }
        }

        internal Result Decode(BinaryBitmap binaryBitmap)
        {
            return Reader.Decode(binaryBitmap);
        }
    }
}