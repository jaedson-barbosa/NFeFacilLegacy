using System.Linq;
using System.Xml.Linq;

namespace NFeFacil.IBGE
{
    internal sealed class ProcessamentoXml
    {
        private XElement Xml;

        public ProcessamentoXml(XElement xml)
        {
            Xml = xml;
        }

        public string GetByName(string nome) => Xml.Element(nome).Value;

        public string GetByIndex(int index)
        {
            var elementos = Xml.Elements();
            return elementos.ElementAt(index).Value;
        }

        public static implicit operator ProcessamentoXml(XElement xml)
        {
            return new ProcessamentoXml(xml);
        }
    }
}
