using Microsoft.EntityFrameworkCore;
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

        public void SalvarMudancas()
        {
            try { Contexto.SaveChanges(); }
            catch (DbUpdateConcurrencyException) { }
            catch (Exception exc) { throw exc; }
        }

        public void Dispose()
        {
            SalvarMudancas();
            Contexto.Dispose();
        }
    }
}
