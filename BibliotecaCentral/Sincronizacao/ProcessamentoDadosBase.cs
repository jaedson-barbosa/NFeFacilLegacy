using BibliotecaCentral.Sincronizacao.Pacotes;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BibliotecaCentral.Sincronizacao
{
    internal static class ProcessamentoDadosBase
    {
        internal static DadosBase Obter()
        {
            using (var Contexto = new AplicativoContext())
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
        }

        internal static void Salvar(DadosBase dados)
        {
            using (var Mudanca = new Repositorio.MudancaOtimizadaBancoDados())
            {
                Mudanca.AdicionarEmitentes(dados.Emitentes);
                Mudanca.AdicionarClientes(dados.Clientes);
                Mudanca.AdicionarMotoristas(dados.Motoristas); ;
                Mudanca.AdicionarProdutos(dados.Produtos);
            }
        }
    }
}
