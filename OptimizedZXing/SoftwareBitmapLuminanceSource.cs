using System;
using System.Runtime.InteropServices;
using Windows.Graphics.Imaging;

namespace OptimizedZXing
{
    [ComImport]
    [Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    unsafe interface IMemoryBufferByteAccess
    {
        void GetBuffer(out byte* buffer, out uint capacity);
    }

    internal class SoftwareBitmapLuminanceSource
    {
        public int Width { get; }
        public int Height { get; }
        public byte[] Matrix { get; }

        SoftwareBitmap softwareBitmap;

        protected const int RChannelWeight = 19562;
        protected const int GChannelWeight = 38550;
        protected const int BChannelWeight = 7424;
        protected const int ChannelWeight = 16;

        public SoftwareBitmapLuminanceSource(SoftwareBitmap softwareBitmap)
        {
            this.softwareBitmap = softwareBitmap;
            Width = softwareBitmap.PixelWidth;
            Height = softwareBitmap.PixelHeight;
            Matrix = new byte[Width * Height];
            CalculateLuminance(softwareBitmap);
        }

        public byte[] GetRow(int y, byte[] row)
        {
            if (row == null || row.Length < Width)
            {
                row = new byte[Width];
            }
            for (int i = 0; i < Width; i++)
                row[i] = Matrix[y * Width + i];
            return row;
        }

        private unsafe void CalculateLuminance(SoftwareBitmap bitmap)
        {
            // Effect is hard-coded to operate on BGRA8 format only
            if (bitmap.BitmapPixelFormat == BitmapPixelFormat.Bgra8)
            {
                // In BGRA8 format, each pixel is defined by 4 bytes
                const int BYTES_PER_PIXEL = 4;

                using (var buffer = bitmap.LockBuffer(BitmapBufferAccessMode.Read))
                using (var reference = buffer.CreateReference())
                {
                    if (reference is IMemoryBufferByteAccess memoryBuffer)
                    {
                        try
                        {
                            // Get a pointer to the pixel buffer
                            memoryBuffer.GetBuffer(out byte* data, out uint capacity);

                            // Get information about the BitmapBuffer
                            var desc = buffer.GetPlaneDescription(0);
                            var luminanceIndex = 0;

                            // Iterate over all pixels
                            for (uint row = 0; row < desc.Height; row++)
                            {
                                for (uint col = 0; col < desc.Width; col++)
                                {
                                    // Index of the current pixel in the buffer (defined by the next 4 bytes, BGRA8)
                                    var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                    // Read the current pixel information into b,g,r channels (leave out alpha channel)
                                    var b = data[currPixel + 0]; // Blue
                                    var g = data[currPixel + 1]; // Green
                                    var r = data[currPixel + 2]; // Red

                                    var luminance = (byte)((RChannelWeight * r + GChannelWeight * g + BChannelWeight * b) >> ChannelWeight);
                                    var alpha = data[currPixel + 3];
                                    luminance = (byte)(((luminance * alpha) >> 8) + (255 * (255 - alpha) >> 8));
                                    Matrix[luminanceIndex] = luminance;
                                    luminanceIndex++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Luminance Source Failed: {0}", ex);
                        }
                    }
                }
            }
        }
    }
}