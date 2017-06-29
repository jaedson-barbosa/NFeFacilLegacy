using System.Linq;
using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    /// <summary>
    /// Grupo Totais da NF-e.
    /// </summary>
    public class Total
    {
        [XmlElement(Order = 0)]
        public ICMSTot ICMSTot { get; set; }

        [XmlElement(Order = 1)]
        public ISSQNtot ISSQNtot { get; set; }

        [XmlElement("retTrib", Order = 2)]
        public RetTrib RetTrib { get; set; }

        public Total() { }
        public Total(List<DetalhesProdutos> produtos)
        {
            ICMSTot = new ICMSTot(from p in produtos
                                  where p.impostos.impostos.Count(x => x is ICMS) > 0
                                  select p);
            ISSQNtot = new ISSQNtot(from p in produtos
                                    where p.impostos.impostos.Count(x => x is ISSQN) > 0
                                    select p);
            RetTrib = new RetTrib();
        }
    }
}
