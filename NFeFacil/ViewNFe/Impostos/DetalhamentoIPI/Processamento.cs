using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoIPI
{
    public sealed class Processamento : ProcessamentoImposto
    {
        DadosIPI dados;

        public override Imposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = dados.Processar(prod.Produto);
            return new Imposto[1] { (IPI)resultado };
        }

        public override bool ValidarDados(ILog log) => !string.IsNullOrEmpty(dados.PreImposto.cEnq);

        public override bool ValidarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var valida = AssociacoesSimples.IPI[detalhamento.TipoCalculo] == Tela?.GetType();
                if (valida)
                {
                    var cst = detalhamento.CST.ToString("00");
                    if (Tela is DetalharAliquota aliq)
                    {
                        dados = new DadosTrib()
                        {
                            CST = cst,
                            Aliquota = aliq.Aliquota,
                            PreImposto = aliq.Conjunto,
                            TipoCalculo = TiposCalculo.PorAliquota
                        };
                    }
                    else if (Tela is DetalharQtde valor)
                    {
                        dados = new DadosTrib()
                        {
                            CST = cst,
                            Valor = valor.ValorUnitario,
                            PreImposto = valor.Conjunto,
                            TipoCalculo = TiposCalculo.PorValor
                        };
                    }
                    else if (Tela is DetalharSimples outr)
                    {
                        dados = new DadosNT
                        {
                            CST = cst,
                            PreImposto = outr.Conjunto
                        };
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
