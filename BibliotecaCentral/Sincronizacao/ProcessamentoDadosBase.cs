using BibliotecaCentral.Sincronizacao.Pacotes;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BibliotecaCentral.Sincronizacao
{
    internal class ProcessamentoDadosBase
    {
        private AplicativoContext Contexto { get; }

        internal ProcessamentoDadosBase(AplicativoContext contexto)
        {
            Contexto = contexto;
        }

        internal DadosBase Obter()
        {
            // O ToList serve para que os dados permaneçam salvos mesmo depois do Dispose
            return new DadosBase
            {
                Emitentes = Contexto.Emitentes.Include(x => x.endereco).ToList(),
                Clientes = Contexto.Clientes.Include(x => x.endereco).ToList(),
                Motoristas = Contexto.Motoristas.ToList(),
                Produtos = Contexto.Produtos.ToList()
            };
        }

        internal void Salvar(DadosBase dados)
        {
            var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(Contexto);
            Mudanca.AdicionarEmitentes(dados.Emitentes);
            Mudanca.AdicionarClientes(dados.Clientes);
            Mudanca.AdicionarMotoristas(dados.Motoristas); ;
            Mudanca.AdicionarProdutos(dados.Produtos);
        }
    }
}
