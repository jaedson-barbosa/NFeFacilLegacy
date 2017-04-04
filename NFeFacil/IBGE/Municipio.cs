using System;
using System.Globalization;
using System.Linq;
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

        public Municipio(XElement xmlMunicípio)
        {
            ProcessamentoXml proc = xmlMunicípio;
            CodigoUF = ushort.Parse(proc.GetByIndex(0), CultureInfo.InvariantCulture);
            Nome = RemoverAcentuacao(proc.GetByIndex(1));
            CodigoMunicípio = long.Parse(proc.GetByIndex(2));
        }

        private static string RemoverAcentuacao(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(x => char.IsLetter(x) || x == ' ')
                .ToArray());
        }
    }
}
