using NFeFacil.ViewNFe;

namespace NFeFacil.ItensBD.Produto
{
    public abstract class ImpostoArmazenado
    {
        public PrincipaisImpostos Tipo { get; set; }
        public string CST { get; set; }
    }
}
