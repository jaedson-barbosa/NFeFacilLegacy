// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public abstract class ExibicaoGenerica
    {
        public string Principal, SecundariaCurta, SecundariaLonga;
        protected object Root;

        public T Convert<T>()
        {
            if (this is ExibicaoEspecifica<T> esp) return esp;
            else return default(T);
        }
    }
}
