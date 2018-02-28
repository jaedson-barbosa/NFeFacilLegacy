using System;

namespace OptimizedZXing
{
    internal class BarcodeReader
    {
        public event Action<Result> ResultFound;
        MultiFormatReader Reader { get; }

        public BarcodeReader(BarcodeFormat format)
        {
            Reader = new MultiFormatReader(format);
        }

        internal Result Decode(SoftwareBitmapLuminanceSource luminanceSource)
        {
            var binaryBitmap = CreateBinarizer(luminanceSource);            
            var result = Reader.Decode(binaryBitmap);
            if (result != null && ResultFound != null)
            {
                ResultFound(result);
            }

            return result;
        }

        static BinaryBitmap CreateBinarizer(SoftwareBitmapLuminanceSource luminanceSource)
        {
            return new BinaryBitmap(luminanceSource);
        }
    }
}