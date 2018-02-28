namespace OptimizedZXing
{
    interface IReader
    {
        Result Decode(BinaryBitmap image);
    }
}