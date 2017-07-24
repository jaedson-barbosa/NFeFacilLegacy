using System;

namespace NFeFacil.Repositorio
{
    public abstract class ConexaoBanco : IDisposable
    {
        internal AplicativoContext Contexto { get; }

        public ConexaoBanco()
        {
            Contexto = new AplicativoContext();
        }

        public void SalvarMudancas()
        {
            Contexto.SaveChanges();
        }

        public void Dispose()
        {
            SalvarMudancas();
            Contexto.Dispose();
        }
    }
}
