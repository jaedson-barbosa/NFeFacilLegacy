using System.Linq;
using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    /// <summary>
    /// Grupo Totais da NF-e.
    /// </summary>
    public class Total
    {
        public Total() { }
        public Total(List<DetalhesProdutos> produtos)
        {
            ICMSTot = new ICMSTot(from p in produtos
                                  where p.impostos.impostos.Count(x => x is ICMS) > 0
                                  select p);
            ISSQNtot = new ISSQNtot(from p in produtos
                                    where p.impostos.impostos.Count(x => x is ISSQN) > 0
                                    select p);
            retTrib = new RetTrib();
        }
        public ICMSTot ICMSTot { get; set; }
        public ISSQNtot ISSQNtot { get; set; }
        public RetTrib retTrib { get; set; }
    }
}
