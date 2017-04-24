using System.Linq;
using System.Xml.Linq;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public struct ConsultarImpostos
    {
        private XElement xml;

        public ConsultarImpostos(XElement xml)
        {
            this.xml = xml;
        }

        public double AgregarValor(string nomeElemento, double valorAlterado)
        {
            var elementos = xml.Elements(nomeElemento);
            if (elementos.Count() > 0)
            {
                var valor = double.Parse(elementos.First().Value);
                return valorAlterado + valor;
            }
            else
            {
                foreach (var item in xml.Elements())
                {
                    var elementosFilho = item.Elements(nomeElemento);
                    if (elementos.Count() > 0)
                    {
                        var valor = double.Parse(elementos.First().Value);
                        return valorAlterado + valor;
                    }
                }
                return 0;
            }
        }
    }
}
