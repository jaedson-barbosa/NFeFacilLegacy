namespace NFeFacil.Fiscal
{
    public interface IControleCriacao
    {
        void Processar(ushort serie, int numero, bool homologacao);
        int ObterMaiorNumero(ushort serie, bool homologacao);
    }
}
