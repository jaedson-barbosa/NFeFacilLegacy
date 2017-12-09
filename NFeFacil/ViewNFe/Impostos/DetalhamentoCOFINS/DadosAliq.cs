using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    sealed class DadosAliq : DadosCOFINS
    {
        public double Aliquota { private get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotalDouble;
            var pCOFINS = Aliquota;
            return new COFINS
            {
                Corpo = new COFINSAliq
                {
                    CST = CST,
                    vBC = vBC.ToString("F2", CulturaPadrao),
                    pCOFINS = pCOFINS.ToString("F4", CulturaPadrao),
                    vCOFINS = (vBC * pCOFINS / 100).ToString("F2", CulturaPadrao)
                }
            };
        }
    }
}
