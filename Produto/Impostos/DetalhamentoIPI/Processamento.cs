using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos.DetalhamentoIPI
{
    public sealed class Processamento : ProcessamentoImposto
    {
        DadosIPI dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var resultado = dados.Processar(prod.Produto);
            return new ImpostoBase[1] { (IPI)resultado };
        }

        public override void ProcessarEntradaDados(Page Tela)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                if (Tela is DetalharAliquota aliq)
                {
                    ProcessarDados(TiposCalculo.PorAliquota, aliq.Aliquota, 0, detalhamento.CST, aliq.Conjunto);
                }
                else if (Tela is DetalharQtde valor)
                {
                    ProcessarDados(TiposCalculo.PorValor, 0, valor.ValorUnitario, detalhamento.CST, valor.Conjunto);
                }
                else if (Tela is DetalharSimples outr)
                {
                    ProcessarDados(TiposCalculo.Inexistente, 0, 0, detalhamento.CST, outr.Conjunto);
                }
            }
        }

        public override void ProcessarDadosProntos()
        {
            if (Detalhamento is ImpSimplesArmazenado imp)
            {
                var bruto = XElement.Parse(imp.IPI).FromXElement<ImpSimplesArmazenado.XMLIPIArmazenado>();
                var ipi = new IPI()
                {
                    cEnq = bruto.CEnq,
                    clEnq = bruto.ClEnq,
                    cSelo = bruto.CSelo,
                    CNPJProd = bruto.CNPJProd,
                    qSelo = bruto.QSelo,
                    Corpo = new IPINT()
                };
                ProcessarDados(imp.TipoCalculo, imp.Aliquota, imp.Valor, imp.CST, ipi);
            }
        }

        void ProcessarDados(TiposCalculo tpCalculo, double aliquota, double valor, int cst, IPI preImposto)
        {
            switch (tpCalculo)
            {
                case TiposCalculo.PorAliquota:
                    dados = new DadosTrib()
                    {
                        CST = cst.ToString("00"),
                        Aliquota = aliquota,
                        PreImposto = preImposto,
                        TipoCalculo = TiposCalculo.PorAliquota
                    };
                    break;
                case TiposCalculo.PorValor:
                    dados = new DadosTrib()
                    {
                        CST = cst.ToString("00"),
                        Valor = valor,
                        PreImposto = preImposto,
                        TipoCalculo = TiposCalculo.PorValor
                    };
                    break;
                default:
                    dados = new DadosNT
                    {
                        CST = cst.ToString("00"),
                        PreImposto = preImposto
                    };
                    break;
            }
            dados.CST = cst.ToString("00");
        }
    }
}
