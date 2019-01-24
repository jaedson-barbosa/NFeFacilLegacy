using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    sealed class DadosAliq : IDadosCOFINS
    {
        public string CST { private get; set; }
        public double Aliquota { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotal;
            if (CST == "01" || CST == "02")
            {
                return new COFINS
                {
                    Corpo = new COFINSAliq(CST, vBC, Aliquota)
                };
            }
            else if (CST == "05")
            {
                return new IImposto[2]
                {
                    new COFINS
                    {
                        Corpo = new COFINSNT(CST)
                    },
                    new COFINSST(vBC, Aliquota, false)
                };
            }
            else
            {
                return new COFINS
                {
                    Corpo = new COFINSOutr(CST, vBC, Aliquota, false)
                };
            }
        }
    }
}
