using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace OptimizedZXing
{
    internal static class Renderer
    {
        public static WriteableBitmap RenderBitmap(BitMatrix matrix, EncodingOptions options)
        {
            var bytes = GetBytes(matrix, options);
            var bmp = new WriteableBitmap(bytes.Width, bytes.Height);
            using (var stream = bmp.PixelBuffer.AsStream())
            {
                var decodedBytes = bytes.Bytes;
                stream.Write(decodedBytes, 0, decodedBytes.Length);
                stream.Flush();
            }
            bmp.Invalidate();
            return bmp;
        }

        public static Rectangle[] RenderUI(BitMatrix matrix, EncodingOptions options)
        {
            Brush Black = new SolidColorBrush(Colors.Black);

            var bytes = GetBytes(matrix, options);

            var blocks = GetUIBlocks(bytes.Height, bytes.Width, bytes.GetOrganizedBooleans());
            var pixelSize = bytes.PixelSize;
            var retorno = new Rectangle[blocks.Count];
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                retorno[i] = new Rectangle()
                {
                    Height = block.Height,
                    Width = block.Width,
                    Margin = new Thickness(block.Column * pixelSize, block.Line * pixelSize, 0, 0),
                    Fill = Black,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
            }
            return retorno;
        }

        static List<UIBlock> GetUIBlocks(int height, int width, bool[,] booleans)
        {
            List<UIBlock> linhas = new List<UIBlock>();
            for (int l = 0; l < height; l++)
            {
                int atHeight = 1, atWidth = 1;
                for (int c = 0; c < width; c += atWidth)
                {
                    atHeight = 1;
                    atWidth = 1;
                    if (!booleans[l, c]) continue;
                    for (int atC = c + 1; atC < width; atC++)
                    {
                        if (booleans[l, atC]) atWidth++;
                        else break;
                    }
                    for (int atL = l + 1; atL < height; atL++)
                    {
                        bool hasSameWidth = true;
                        for (int atC = c; atC < c + atWidth; atC++)
                        {
                            if (!booleans[atL, atC])
                            {
                                hasSameWidth = false;
                                break;
                            }
                        }
                        if (hasSameWidth)
                        {
                            atHeight++;
                            for (int atC = c; atC < c + atWidth; atC++)
                            {
                                booleans[atL, atC] = false;
                            }
                        }
                        else break;
                    }
                    linhas.Add(new UIBlock(atWidth, atHeight, c, l));
                }
            }
            return linhas;
        }

        sealed class UIBlock
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int Column { get; set; }
            public int Line { get; set; }

            public UIBlock(int width, int height, int column, int line)
            {
                Width = width;
                Height = height;
                Column = column;
                Line = line;
            }
        }

        static BytesImage GetBytes(BitMatrix matrix, EncodingOptions options)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            int pixelsize = 1;

            if (options != null)
            {
                if (options.Width > width)
                {
                    width = options.Width;
                }
                if (options.Height > height)
                {
                    height = options.Height;
                }
                // calculating the scaling factor
                pixelsize = width / matrix.Width;
                if (pixelsize > height / matrix.Height)
                {
                    pixelsize = height / matrix.Height;
                }
            }

            byte foreground = 0;
            byte background = 255;
            var bytes = new BytesImage(width, height, pixelsize);
            for (int y = 0; y < matrix.Height; y++)
            {
                for (var pixelsizeHeight = 0; pixelsizeHeight < pixelsize; pixelsizeHeight++)
                {
                    for (var x = 0; x < matrix.Width; x++)
                    {
                        var color = matrix[x, y] ? foreground : background;
                        for (var pixelsizeWidth = 0; pixelsizeWidth < pixelsize; pixelsizeWidth++)
                        {
                            bytes.Write4Bits(color);
                        }
                    }
                    for (var x = pixelsize * matrix.Width; x < width; x++)
                    {
                        bytes.Write4Bits(background);
                    }
                }
            }
            for (int y = matrix.Height * pixelsize; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    bytes.Write4Bits(background);
                }
            }
            return bytes;
        }
    }

    sealed class BytesImage
    {
        public int Width { get; }
        public int Height { get; }
        public int PixelSize { get; }
        public byte[] Bytes { get; }
        public bool[] Booleans { get; }

        public BytesImage(int width, int height, int pixelSize)
        {
            Width = width;
            Height = height;
            PixelSize = pixelSize;
            Bytes = new byte[width * height * 4];
            Booleans = new bool[width * height];
        }

        int lastIndex = 0;
        public void Write4Bits(byte value)
        {
            Bytes[lastIndex] = value;
            Bytes[++lastIndex] = value;
            Bytes[++lastIndex] = value;
            Bytes[++lastIndex] = 255;
            lastIndex++;
            WriteBool(value == 0);
        }

        int lastBoolIndex = 0;
        void WriteBool(bool value)
        {
            Booleans[lastBoolIndex++] = value;
        }

        public bool[,] GetOrganizedBooleans()
        {
            var retorno = new bool[Width, Height];
            for (int i = 0, c = 0, l = 0; i < Booleans.Length; i++, c++)
            {
                retorno[l, c] = Booleans[i];
                if (c == Width - 1)
                {
                    c = -1;
                    l++;
                }
            }
            return retorno;
        }
    }
}
