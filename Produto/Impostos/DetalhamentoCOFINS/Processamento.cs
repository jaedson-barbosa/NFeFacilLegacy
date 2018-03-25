using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoCOFINS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosCOFINS dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var resultado = dados.Processar(prod.Produto);
            if (resultado is ImpostoBase[] list) return list;
            else return new ImpostoBase[1] { (COFINS)resultado };
        }

        public override bool ValidarDados() => dados != null;

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                if (Tela is DetalharAliquota aliq)
                {
                    ProcessarDados(TiposCalculo.PorAliquota, aliq.Aliquota, 0, detalhamento.CST);
                }
                else if (Tela is DetalharQtde valor)
                {
                    ProcessarDados(TiposCalculo.PorValor, 0, valor.Valor, detalhamento.CST);
                }
                else
                {
                    ProcessarDados(TiposCalculo.Inexistente, 0, 0, detalhamento.CST);
                }
            }
            else if (Detalhamento is DadoPronto pronto)
            {
                ProcessarDadosProntos(pronto.ImpostoPronto);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto)
        {
            if (imposto is ImpSimplesArmazenado imp)
            {
                ProcessarDados(imp.TipoCalculo, imp.Aliquota, imp.Valor, imp.CST);
            }
        }

        void ProcessarDados(TiposCalculo tpCalculo, double aliquota, double valor, int cst)
        {
            switch (tpCalculo)
            {
                case TiposCalculo.PorAliquota:
                    dados = new DadosAliq()
                    {
                        Aliquota = aliquota
                    };
                    break;
                case TiposCalculo.PorValor:
                    dados = new DadosQtde()
                    {
                        Valor = valor
                    };
                    break;
                default:
                    dados = new DadosNT();
                    break;
            }
            dados.CST = cst.ToString("00");
        }
    }
}
