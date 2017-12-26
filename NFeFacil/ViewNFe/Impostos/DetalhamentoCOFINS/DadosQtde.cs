using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
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
                    Corpo = new COFINSQtde
                    {
                        CST = CST,
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vCOFINS = ToStr(qBCProd * Valor)
                    }
                };
            }
            else if (CST == "05")
            {
                return new ImpostoBase[2]
                {
                    new COFINS
                    {
                        Corpo = new COFINSNT()
                        {
                            CST = CST
                        }
                    },
                    new COFINSST
                    {
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vCOFINS = ToStr(qBCProd * Valor)
                    }
                };
            }
            else
            {
                return new COFINS
                {
                    Corpo = new COFINSOutr
                    {
                        CST = CST,
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vCOFINS = ToStr(qBCProd * Valor)
                    }
                };
            }
        }
    }
}
