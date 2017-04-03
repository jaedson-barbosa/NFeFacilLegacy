namespace NFeFacil.ViewModel
{
    public interface IObterPropriedade<out Retorno>
    {
        bool TipoEsperado(object elemento);
        Retorno ObterPropriedade(object elemento);
    }
}
