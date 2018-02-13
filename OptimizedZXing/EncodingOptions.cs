namespace OptimizedZXing
{
    public class EncodingOptions
    {
        public int Height { get; set; }

        /// <summary>
        /// Specifies the width of the barcode image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Don't put the content string into the output image.
        /// </summary>
        public bool PureBarcode { get; set; }

        /// <summary>
        /// Specifies margin, in pixels, to use when generating the barcode. The meaning can vary
        /// by format; for example it controls margin before and after the barcode horizontally for
        /// most 1D formats.
        /// </summary>
        public int Margin { get; set; }
    }
}
