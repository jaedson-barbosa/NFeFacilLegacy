namespace OptimizedZXing
{
    public interface IWriter
    {
        BitMatrix Encode(string contents, BarcodeFormat format, int width, int height, int margin);
    }
}
