using System;

namespace BibliotecaCentral.Repositorio
{
    public abstract class ConexaoBanco : IDisposable
    {
        protected AplicativoContext Contexto { get; }

        public ConexaoBanco()
        {
            Contexto = new AplicativoContext();
        }

        protected ConexaoBanco(AplicativoContext contexto)
        {
            Contexto = contexto;
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
