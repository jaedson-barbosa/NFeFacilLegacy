using System.Linq;
using System.Xml.Linq;

namespace BibliotecaCentral.IBGE
{
    internal sealed class ProcessamentoXml
    {
        private XElement Xml;

        internal ProcessamentoXml(XElement xml)
        {
            Xml = xml;
        }

        internal string GetByName(string nome)
        {
            return Xml.Element(nome).Value;
        }

        internal string GetByIndex(int index)
        {
            var elementos = Xml.Elements();
            return elementos.ElementAt(index).Value;
        }
    }
}
