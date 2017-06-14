namespace CodigoBarras
{
    internal abstract class BarcodeCommon
    {
        protected string RawData { get; }

        protected BarcodeCommon(string rawData)
        {
            RawData = rawData;
        }

        protected bool CheckNumericOnly()
        {
            if (RawData != null)
            {
                for (int i = 0; i < RawData.Length; i++)
                {
                    if (!char.IsNumber(RawData[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
