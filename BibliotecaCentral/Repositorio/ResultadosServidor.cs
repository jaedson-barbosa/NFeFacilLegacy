using System.Collections.Generic;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class ResultadosServidor : ConexaoBanco
    {
        public IEnumerable<ResultadoSincronizacaoServidor> Registro => Contexto.ResultadosServidor;
        public void Adicionar(ResultadoSincronizacaoServidor dado) => Contexto.Add(dado);
        public void Atualizar(ResultadoSincronizacaoServidor dado) => Contexto.Update(dado);
        public void Remover(ResultadoSincronizacaoServidor dado) => Contexto.Remove(dado);
    }
}
