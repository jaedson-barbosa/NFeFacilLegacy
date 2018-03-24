using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto
{
    public sealed class Impostos
    {
        public Impostos() { }
        public Impostos(IEnumerable<ImpostoBase> lista)
        {
            impostos = lista.ToList();
        }

        [DescricaoPropriedade("Valor total dos tributos")]
        public string vTotTrib { get; set; }

        [XmlElement(nameof(ICMS), Type = typeof(ICMS)),
            XmlElement(nameof(COFINS), Type = typeof(COFINS)),
            XmlElement(nameof(COFINSST), Type = typeof(COFINSST)),
            XmlElement(nameof(II), Type = typeof(II)),
            XmlElement(nameof(IPI), Type = typeof(IPI)),
            XmlElement(nameof(ISSQN), Type = typeof(ISSQN)),
            XmlElement(nameof(PIS), Type = typeof(PIS)),
            XmlElement(nameof(PISST), Type = typeof(PISST))]
        public List<ImpostoBase> impostos { get; set; } = new List<ImpostoBase>();
    }
}
