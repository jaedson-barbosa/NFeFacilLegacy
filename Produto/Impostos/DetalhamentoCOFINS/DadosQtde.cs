using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    sealed class DadosQtde : IDadosCOFINS
    {
        public string CST { private get; set; }
        public double Valor { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var qBCProd = prod.QuantidadeComercializada;
            if (CST == "03")
            {
                return new COFINS
                {
                    Corpo = new COFINSQtde(CST, qBCProd, Valor)
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
                    new COFINSST(qBCProd, Valor, true)
                };
            }
            else
            {
                return new COFINS
                {
                    Corpo = new COFINSOutr(CST, qBCProd, Valor, true)
                };
            }
        }
    }
}
