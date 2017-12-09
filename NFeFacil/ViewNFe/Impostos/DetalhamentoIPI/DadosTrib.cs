using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewNFe.CaixasImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoIPI
{
    class DadosTrib : DadosIPI
    {
        public double Aliquota { protected get; set; }
        public double Valor { protected get; set; }
        public TiposCalculo TipoCalculo { protected get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            ComumIPI corpo;
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotalDouble;
                var pIPI = Aliquota;
                corpo = new IPITrib
                {
                    vBC = vBC.ToString("F2", CulturaPadrao),
                    pIPI = pIPI.ToString("F4", CulturaPadrao),
                    vIPI = (vBC * pIPI / 100).ToString("F2", CulturaPadrao)
                };
            }
            else
            {
                var qUnid = prod.QuantidadeComercializada;
                var vUnid = Valor;
                corpo = new IPITrib
                {
                    qUnid = qUnid.ToString("F4", CulturaPadrao),
                    vUnid = vUnid.ToString("F4", CulturaPadrao),
                    vIPI = (qUnid * vUnid).ToString("F2", CulturaPadrao)
                };
            }
            PreImposto.Corpo = corpo;
            return PreImposto;
        }
    }
}
