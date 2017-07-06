using System;
using CodigoBarras.Symbologies;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace CodigoBarras
{
    /// <summary>
    /// Generates a barcode image of a specified symbology from a string of data.
    /// </summary>
    public class Barcode
    {
        private IBarcode iBarcode;
        /// <summary>
        /// Gets or sets the raw data to encode.
        /// </summary>
        public string RawData { get; }
        /// <summary>
        /// Gets the encoded value.
        /// </summary>
        public string EncodedValue { get; private set; }

        public Barcode(string data)
        {
            RawData = data;
        }

        /// <summary>
        /// Encodes the raw data into binary form representing bars and spaces.
        /// </summary>
        public Rectangle[] Encode(Code128Types type)
        {
            //make sure there is something to encode
            if (RawData.Trim() == "") 
                throw new Exception("EENCODE-1: Input data not allowed to be blank.");

            EncodedValue = "";

            iBarcode = new Code128(RawData, type);
            EncodedValue = iBarcode.EncodedValue;

            var bars = new Rectangle[EncodedValue.Length];

            //draw image
            for (int pos = 0; pos < EncodedValue.Length; pos++)
            {
                bars[pos] = new Rectangle
                {
                    Fill = new SolidColorBrush(EncodedValue[pos] == '1' ? Colors.Black : Colors.White),
                    Width = 1,
                    Height = 30,
                    UseLayoutRounding = false
                };
            }

            return bars;
        }
    }
}
