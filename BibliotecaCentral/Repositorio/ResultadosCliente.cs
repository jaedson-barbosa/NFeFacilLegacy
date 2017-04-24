using System.Collections.Generic;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class ResultadosCliente : ConexaoBanco<ResultadoSincronizacaoCliente, ResultadoSincronizacaoCliente>
    {
        public IEnumerable<ResultadoSincronizacaoCliente> Registro => Contexto.ResultadosCliente;
        public void Adicionar(ResultadoSincronizacaoCliente dado) => Contexto.Add(dado);
        public void Atualizar(ResultadoSincronizacaoCliente dado) => Contexto.Update(dado);
        public void Remover(ResultadoSincronizacaoCliente dado) => Contexto.Remove(dado);
    }
}
