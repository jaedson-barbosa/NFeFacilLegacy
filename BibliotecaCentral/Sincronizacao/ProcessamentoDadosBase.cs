using BibliotecaCentral.Sincronizacao.Pacotes;
using System;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao
{
    internal class ProcessamentoDadosBase : IDisposable
    {
        private AplicativoContext Contexto { get; }
        internal ProcessamentoDadosBase()
        {
            Contexto = new AplicativoContext();
        }

        public DadosBase Obter()
        {
            return new DadosBase
            {
                Emitentes = Contexto.Emitentes,
                Clientes = Contexto.Clientes,
                Motoristas = Contexto.Motoristas,
                Produtos = Contexto.Produtos
            };
        }

        public async Task SalvarAsync(DadosBase dados)
        {
            Contexto.Emitentes.AddRange(dados.Emitentes);
            Contexto.Clientes.AddRange(dados.Clientes);
            Contexto.Motoristas.AddRange(dados.Motoristas); ;
            Contexto.Produtos.AddRange(dados.Produtos);
            await Contexto.SaveChangesAsync();
        }

        public void Dispose() => Contexto.Dispose();
    }
}
