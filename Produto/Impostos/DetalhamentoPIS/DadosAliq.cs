using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoPIS
{
    sealed class DadosAliq : IDadosPIS
    {
        public string CST { private get; set; }
        public double Aliquota { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var vBC = prod.ValorTotal;
            if (CST == "01" || CST == "02")
            {
                return new PIS
                {
                    Corpo = new PISAliq(CST, vBC, Aliquota)
                };
            }
            else if (CST == "05")
            {
                return new IImposto[2]
                {
                    new PIS
                    {
                        Corpo = new PISNT(CST)
                    },
                    new PISST(vBC, Aliquota, false)
                };
            }
            else
            {
                return new PIS
                {
                    Corpo = new PISOutr(CST, vBC, Aliquota, false)
                };
            }
        }
    }
}
