namespace OptimizedZXing
{
    public class MobileBarcodeScanningOptions
    {
        public MobileBarcodeScanningOptions(BarcodeFormat format)
        {
            Format = format;
        }

        public int DelayBetweenContinuousScans { get; set; }
        readonly BarcodeFormat Format;

        internal BarcodeReader BuildBarcodeReader()
        {
            return new BarcodeReader(Format);
        }
    }
}