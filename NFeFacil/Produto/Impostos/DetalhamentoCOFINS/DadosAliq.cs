using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoCOFINS
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
                    Corpo = new COFINSAliq
                    {
                        CST = CST,
                        vBC = ToStr(vBC),
                        pCOFINS = ToStr(Aliquota, "F4"),
                        vCOFINS = ToStr(vBC * Aliquota / 100)
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
                        vBC = ToStr(vBC),
                        pCOFINS = ToStr(Aliquota, "F4"),
                        vCOFINS = ToStr(vBC * Aliquota/ 100)
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
                        vBC = ToStr(vBC),
                        pCOFINS = ToStr(Aliquota, "F4"),
                        vCOFINS = ToStr(vBC * Aliquota / 100)
                    }
                };
            }
        }
    }
}
