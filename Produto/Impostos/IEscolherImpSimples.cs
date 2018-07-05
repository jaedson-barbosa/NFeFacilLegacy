namespace Venda.Impostos
{
    public interface IEscolherImpSimples
    {
        string CST { get; }
        TiposCalculo TipoCalculo { get; }
    }
}