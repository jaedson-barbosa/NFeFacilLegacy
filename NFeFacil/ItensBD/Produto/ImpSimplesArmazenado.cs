using NFeFacil.ViewNFe.Impostos;

namespace NFeFacil.ItensBD.Produto
{
    public sealed class ImpSimplesArmazenado : ImpostoArmazenado
    {
        internal TiposCalculo TipoCalculo { get; set; }
        public double Aliquota { get; set; }
        public double Valor { get; set; }
    }
}
