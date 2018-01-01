using NFeFacil.Produto.Impostos;

namespace NFeFacil.ItensBD.Produto
{
    public sealed class ImpSimplesArmazenado : ImpostoArmazenado
    {
        public TiposCalculo TipoCalculo { get; set; }
        public double Aliquota { get; set; }
        public double Valor { get; set; }

        public string IPI { get; set; }

        public struct XMLIPIArmazenado
        {
            public string ClEnq { get; set; }
            public string CNPJProd { get; set; }
            public string CSelo { get; set; }
            public string QSelo { get; set; }
            public string CEnq { get; set; }
        }
    }
}
