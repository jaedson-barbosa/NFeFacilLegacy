namespace OptimizedZXing
{
    public class Result
    {
        public string Text { get; }
        public byte[] RawBytes { get; }
        public ResultPoint[] ResultPoints { get; }
        public BarcodeFormat BarcodeFormat { get; }

        public Result(string text, byte[] rawBytes, ResultPoint[] resultPoints, BarcodeFormat format)
        {
            Text = text;
            RawBytes = rawBytes;
            ResultPoints = resultPoints;
            BarcodeFormat = format;
        }
    }
}
