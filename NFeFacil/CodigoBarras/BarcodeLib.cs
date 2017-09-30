using System;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace NFeFacil.CodigoBarras
{
    /// <summary>
    /// Generates a barcode image of a specified symbology from a string of data.
    /// </summary>
    public class Barcode
    {
        private Code128Types type;
        private Code128Symbologie iBarcode;
        /// <summary>
        /// Gets or sets the raw data to encode.
        /// </summary>
        public string RawData { get; }
        /// <summary>
        /// Gets the encoded value.
        /// </summary>
        public string EncodedValue { get; private set; }

        public Barcode(string data, Code128Types type)
        {
            RawData = data;
            this.type = type;
        }

        public void Preencode()
        {
            if (string.IsNullOrWhiteSpace(RawData))
            {
                throw new Exception("EENCODE-1: Input data not allowed to be blank.");
            }

            iBarcode = new Code128Symbologie(RawData, type);
            EncodedValue = iBarcode.EncodedValue;
        }

        /// <summary>
        /// Encodes the raw data into binary form representing bars and spaces.
        /// </summary>
        public Rectangle[] Encode(int width, double height)
        {
            if (string.IsNullOrWhiteSpace(EncodedValue))
            {
                Preencode();
            }

            var bars = new Rectangle[EncodedValue.Length];

            //draw image
            for (int pos = 0; pos < EncodedValue.Length; pos++)
            {
                bars[pos] = new Rectangle
                {
                    Fill = new SolidColorBrush(EncodedValue[pos] == '1' ? Colors.Black : Colors.White),
                    Width = width,
                    Height = height,
                    UseLayoutRounding = false
                };
            }

            return bars;
        }
    }
}
