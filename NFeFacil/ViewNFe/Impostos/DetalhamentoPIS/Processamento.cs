using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosPIS dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var resultado = dados.Processar(prod.Produto);
            if (resultado is ImpostoBase[] list) return list;
            else return new ImpostoBase[1] { (PIS)resultado };
        }

        public override bool ValidarDados() => true;

        public override bool ValidarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                if (AssociacoesSimples.PIS.ContainsKey(detalhamento.CST)
                    && AssociacoesSimples.PIS[detalhamento.CST] == Tela?.GetType())
                {
                    if (Tela is DetalharAliquota aliq)
                    {
                        dados = new DadosAliq()
                        {
                            Aliquota = aliq.Aliquota
                        };
                    }
                    else if (Tela is DetalharQtde valor)
                    {
                        dados = new DadosQtde()
                        {
                            Valor = valor.Valor
                        };
                    }
                    else
                    {
                        dados = new DadosNT();
                    }
                }
                else
                {
                    if (detalhamento.TipoCalculo == TiposCalculo.PorAliquota && Tela is DetalharAliquota aliq)
                    {
                        dados = new DadosAliq()
                        {
                            Aliquota = aliq.Aliquota
                        };
                    }
                    else if (detalhamento.TipoCalculo == TiposCalculo.PorValor && Tela is DetalharQtde valor)
                    {
                        dados = new DadosQtde()
                        {
                            Valor = valor.Valor
                        };
                    }
                    else
                    {
                        return false;
                    }
                }
                dados.CST = detalhamento.CST.ToString("00");
                return true;
            }
            return false;
        }
    }
}
