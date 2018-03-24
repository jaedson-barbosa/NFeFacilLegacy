using System.Linq;
using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesDetalhes.PartesTotal;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Xml.Serialization;
using NFeFacil.View;

namespace NFeFacil.ModeloXML.PartesDetalhes
{
    /// <summary>
    /// Grupo Totais da NF-e.
    /// </summary>
    public class Total
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Total do ICMS")]
        public ICMSTot ICMSTot { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Total do ISSQN")]
        public ISSQNtot ISSQNtot { get; set; }

        [XmlElement("retTrib", Order = 2), DescricaoPropriedade("Total da retenção tributária")]
        public RetTrib RetTrib { get; set; }

        public Total() { }
        public Total(List<DetalhesProdutos> produtos)
        {
            ICMSTot = new ICMSTot(from p in produtos
                                  where p.Impostos.impostos.Count(x => x is ICMS) > 0
                                  select p);
            ISSQNtot = new ISSQNtot(from p in produtos
                                    where p.Impostos.impostos.Count(x => x is ISSQN) > 0
                                    select p);
        }
    }
}
