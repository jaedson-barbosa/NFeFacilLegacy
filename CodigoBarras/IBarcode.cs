namespace CodigoBarras
{
    /// <summary>
    ///  Barcode interface for symbology layout.
    /// </summary>
    internal interface IBarcode
    {
        string EncodedValue { get; }
    }
}
