using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace NFeFacil.IBGE
{
    public class Municipio
    {
        public ushort CodigoUF { get; set; }
        public string Nome { get; set; }
        public long CodigoMunicípio { get; set; }

        public Municipio() { }

        public Municipio(string codigoUF, string nome, string codigoMunicípio)
        {
            CodigoUF = ushort.Parse(codigoUF);
            Nome = nome;
            CodigoMunicípio = long.Parse(codigoMunicípio, System.Globalization.CultureInfo.InvariantCulture);
        }

        public Municipio(XElement xmlMunicípio)
        {
            ProcessamentoXml proc = xmlMunicípio;
            CodigoUF = ushort.Parse(proc.GetByIndex(0), System.Globalization.CultureInfo.InvariantCulture);
            Nome = RemoveAccents(proc.GetByIndex(1));
            CodigoMunicípio = long.Parse(proc.GetByIndex(2));
        }

        public static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
