using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoPIS
{
    sealed class DadosQtde : IDadosPIS
    {
        public string CST { private get; set; }
        public double Valor { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var qBCProd = prod.QuantidadeComercializada;
            if (CST == "03")
            {
                return new PIS
                {
                    Corpo = new PISQtde
                    {
                        CST = CST,
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vPIS = ToStr(qBCProd * Valor)
                    }
                };
            }
            else if (CST == "05")
            {
                return new ImpostoBase[2]
                {
                    new PIS
                    {
                        Corpo = new PISNT()
                        {
                            CST = CST
                        }
                    },
                    new PISST
                    {
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vPIS = ToStr(qBCProd * Valor)
                    }
                };
            }
            else
            {
                return new PIS
                {
                    Corpo = new PISOutr
                    {
                        CST = CST,
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vPIS = ToStr(qBCProd * Valor)
                    }
                };
            }
        }
    }
}
