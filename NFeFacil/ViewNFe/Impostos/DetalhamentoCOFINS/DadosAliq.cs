using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    sealed class DadosAliq : DadosCOFINS
    {
        public double Aliquota { private get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotal;
            var pCOFINS = Aliquota;
            return new COFINS
            {
                Corpo = new COFINSAliq
                {
                    CST = CST,
                    vBC = ToStr(vBC),
                    pCOFINS = ToStr(pCOFINS, "F4"),
                    vCOFINS = ToStr(vBC * pCOFINS / 100);
                }
            };
        }
    }
}
