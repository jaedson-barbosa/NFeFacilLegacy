using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosAliq : DadosPIS
    {
        public double Aliquota { private get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotal;
            var pPIS = Aliquota;
            return new PIS
            {
                Corpo = new PISAliq
                {
                    CST = CST,
                    vBC = vBC.ToString("F2", CulturaPadrao),
                    pPIS = pPIS.ToString("F4", CulturaPadrao),
                    vPIS = (vBC * pPIS / 100).ToString("F2", CulturaPadrao)
                }
            };
        }
    }
}
