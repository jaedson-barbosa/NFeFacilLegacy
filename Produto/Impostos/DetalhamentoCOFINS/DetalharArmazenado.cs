using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    public sealed class DetalharArmazenado : IProcessamentoImposto
    {
        readonly ImpSimplesArmazenado ImpArmazenado;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.COFINS;

        public DetalharArmazenado(ImpSimplesArmazenado impArmazenado) => ImpArmazenado = impArmazenado;

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            object resultado;
            switch (ImpArmazenado.TipoCalculo)
            {
                case TiposCalculo.PorAliquota:
                    resultado = new DadosAliq()
                    {
                        Aliquota = ImpArmazenado.Aliquota,
                        CST = ImpArmazenado.CST.ToString("00")
                    }.Processar(prod.Produto);
                    break;
                case TiposCalculo.PorValor:
                    resultado = new DadosQtde()
                    {
                        Valor = ImpArmazenado.Valor,
                        CST = ImpArmazenado.CST.ToString("00")
                    }.Processar(prod.Produto);
                    break;
                default:
                    resultado = new DadosNT()
                    {
                        CST = ImpArmazenado.CST.ToString("00")
                    }.Processar(prod.Produto);
                    break;
            }
            if (resultado is IImposto[] list) return list;
            else return new IImposto[1] { (COFINS)resultado };
        }
    }
}
