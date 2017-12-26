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
            if (CST == "01" || CST == "02")
            {
                return new PIS
                {
                    Corpo = new PISAliq
                    {
                        CST = CST,
                        vBC = ToStr(vBC),
                        pPIS = ToStr(Aliquota, "F4"),
                        vPIS = ToStr(vBC * Aliquota / 100)
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
                        vBC = ToStr(vBC),
                        pPIS = ToStr(Aliquota, "F4"),
                        vPIS = ToStr(vBC * Aliquota/ 100)
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
                        vBC = ToStr(vBC),
                        pPIS = ToStr(Aliquota, "F4"),
                        vPIS = ToStr(vBC * Aliquota / 100)
                    }
                };
            }
        }
    }
}
