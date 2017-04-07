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
        public int Codigo { get; set; }

        public Municipio() { }

        public Municipio(XElement xmlMunicípio)
        {
            ProcessamentoXml proc = xmlMunicípio;
            CodigoUF = ushort.Parse(proc.GetByIndex(0), CultureInfo.InvariantCulture);
            Nome = RemoverAcentuacao(proc.GetByIndex(1));
            Codigo = int.Parse(proc.GetByIndex(2));
        }

        public static bool operator ==(Municipio mun1, Municipio mun2) {
            return Equals(mun1, mun2);
        }

        public static bool operator !=(Municipio mun1, Municipio mun2)
        {
            return !Equals(mun1, mun2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Municipio mun)
            {
                return GetHashCode() == mun.GetHashCode();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public override int GetHashCode()
        {
            return CodigoUF * Codigo * Nome.Length;
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
