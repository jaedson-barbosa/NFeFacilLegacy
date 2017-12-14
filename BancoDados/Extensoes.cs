using System.Globalization;

namespace NFeFacil
{
    static class Extensoes
    {
        public static double ToDouble(this string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : double.Parse(str, CultureInfo.InvariantCulture);
        }

        static CultureInfo culturaPadrao = CultureInfo.InvariantCulture;
        public static string ToStr(double valor) => valor.ToString("F2", culturaPadrao);
        public static double Parse(string str) => double.Parse(str, NumberStyles.Number, culturaPadrao);
    }
}
