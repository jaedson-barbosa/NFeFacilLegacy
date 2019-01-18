// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    sealed class ExibicaoEspecifica<Tipo> : ExibicaoGenerica
    {
        public ExibicaoEspecifica(Tipo old, string principal, string secCurta, string secLonga)
        {
            Principal = principal;
            SecundariaCurta = secCurta;
            SecundariaLonga = secLonga;
            Root = old;
        }

        public static implicit operator Tipo(ExibicaoEspecifica<Tipo> old) => (Tipo)old.Root;
    }
}
