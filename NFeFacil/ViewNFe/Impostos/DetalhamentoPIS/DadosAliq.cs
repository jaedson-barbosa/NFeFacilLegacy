using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosAliq : IDadosPIS
    {
        public string CST { private get; set; }
        public double Aliquota { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotal;
            var pPIS = Aliquota;
            return new PIS
            {
                Corpo = new PISAliq
                {
                    CST = CST,
                    vBC = ToStr(vBC),
                    pPIS = ToStr(pPIS, "F4"),
                    vPIS = ToStr(vBC * pPIS / 100)
                }
            };
        }
    }
}
