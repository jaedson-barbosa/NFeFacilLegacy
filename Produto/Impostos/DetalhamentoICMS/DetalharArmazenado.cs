using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoICMS
{
    public sealed class DetalharArmazenado : IProcessamentoImposto
    {
        readonly ICMSArmazenado Armazenado;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public DetalharArmazenado(ICMSArmazenado armazenado) => Armazenado = armazenado;

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            return new IImposto[1] { new ICMS
            {
                Corpo = (ComumICMS)(Armazenado.IsRegimeNormal
                ? (IDadosICMS)Armazenado.RegimeNormal
                : Armazenado.SimplesNacional).Processar(prod)
            }};
        }
    }
}
