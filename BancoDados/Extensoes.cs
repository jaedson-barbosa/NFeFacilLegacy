using System.Globalization;

namespace NFeFacil
{
    static class Extensoes
    {
        static CultureInfo culturaPadrao = CultureInfo.InvariantCulture;
        public static string ToStr(double valor) => valor.ToString("F2", culturaPadrao);
        public static double Parse(string str) => double.Parse(str, NumberStyles.Number, culturaPadrao);
        public static bool TryParse(string str, out double valor) => double.TryParse(str, NumberStyles.Number, culturaPadrao, out valor);
    }
}
