using Venda.Impostos;

namespace Venda
{
    public sealed class ImpSimplesArmazenado : ImpostoArmazenado
    {
        public TiposCalculo TipoCalculo { get; set; }
        public double Aliquota { get; set; }
        public double Valor { get; set; }

        public string IPI { get; set; }

        public struct XMLIPIArmazenado
        {
            public string CNPJProd { get; set; }
            public string CSelo { get; set; }
            public string QSelo { get; set; }
            public string CEnq { get; set; }
        }
    }
}
