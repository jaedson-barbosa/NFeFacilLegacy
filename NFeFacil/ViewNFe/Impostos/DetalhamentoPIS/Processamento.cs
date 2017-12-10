using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        DadosPIS dados;

        public override Imposto[] Processar(ProdutoOuServico prod)
        {
            var resultado = dados.Processar(prod);
            if (resultado is Imposto[] list) return list;
            else return new Imposto[1] { (PIS)resultado };
        }

        public override bool ValidarDados(ILog log) => true;

        public override bool ValidarEntradaDados(ILog log)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var valida = (AssociacoesSimples.PIS.ContainsKey(detalhamento.CST)
                    && AssociacoesSimples.PIS[detalhamento.CST] == Tela?.GetType())
                    || AssociacoesSimples.PISPadrao == Tela?.GetType();
                if (valida)
                {
                    var cst = detalhamento.CST.ToString("00");
                    if (Tela is DetalharAliquota aliq)
                    {
                        dados = new DadosAliq()
                        {
                            CST = cst,
                            Aliquota = aliq.Aliquota
                        };
                    }
                    else if (Tela is DetalharQtde valor)
                    {
                        dados = new DadosQtde()
                        {
                            CST = cst,
                            Valor = valor.Valor
                        };
                    }
                    else if (Tela is DetalharAmbos outr)
                    {
                        if (detalhamento.CST == 5) dados = new DadosST()
                        {
                            CST = cst,
                            Aliquota = outr.Aliquota,
                            Valor = outr.Valor,
                            TipoCalculo = outr.TipoCalculo
                        };
                        else dados = new DadosOutr()
                        {
                            CST = cst,
                            Aliquota = outr.Aliquota,
                            Valor = outr.Valor,
                            TipoCalculo = outr.TipoCalculo
                        };
                    }
                    else
                    {
                        dados = new DadosNT();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
