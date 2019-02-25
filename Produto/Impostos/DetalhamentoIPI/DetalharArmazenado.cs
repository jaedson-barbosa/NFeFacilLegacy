using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Xml.Linq;

namespace Venda.Impostos.DetalhamentoIPI
{
    public sealed class DetalharArmazenado : IProcessamentoImposto
    {
        readonly ImpSimplesArmazenado ImpArmazenado;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.IPI;

        public DetalharArmazenado(ImpSimplesArmazenado impArmazenado) => ImpArmazenado = impArmazenado;

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var bruto = XElement.Parse(ImpArmazenado.IPI).FromXElement<ImpSimplesArmazenado.XMLIPIArmazenado>();
            var ipi = new IPI()
            {
                CodigoEnquadramento = bruto.CEnq,
                CodigoSelo = bruto.CSelo,
                CNPJProd = bruto.CNPJProd,
                QuantidadeSelos = bruto.QSelo,
                Corpo = new IPINT(ImpArmazenado.CST.ToString("00"))
            };

            object resultado;
            switch (ImpArmazenado.TipoCalculo)
            {
                case TiposCalculo.PorAliquota:
                    resultado = new DadosTrib()
                    {
                        CST = ImpArmazenado.CST.ToString("00"),
                        Aliquota = ImpArmazenado.Aliquota,
                        PreImposto = ipi,
                        TipoCalculo = TiposCalculo.PorAliquota
                    }.Processar(prod.Produto);
                    break;
                case TiposCalculo.PorValor:
                    resultado = new DadosTrib()
                    {
                        CST = ImpArmazenado.CST.ToString("00"),
                        Valor = ImpArmazenado.Valor,
                        PreImposto = ipi,
                        TipoCalculo = TiposCalculo.PorValor
                    }.Processar(prod.Produto);
                    break;
                default:
                    resultado = new DadosNT
                    {
                        CST = ImpArmazenado.CST.ToString("00"),
                        PreImposto = ipi
                    }.Processar(prod.Produto);
                    break;
            }
            return new IImposto[1] { (IPI)resultado };
        }
    }
}
