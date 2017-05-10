using BibliotecaCentral.Sincronizacao.Pacotes;
using Microsoft.EntityFrameworkCore;
using System;
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

        internal DadosBase Obter(DateTime minimo)
        {
            // O ToList serve para que os dados permaneçam salvos mesmo depois do Dispose
            return new DadosBase
            {
                Emitentes = Contexto.Emitentes
                .Where(x => x.UltimaData > minimo)
                .Include(x => x.endereco)
                .ToList(),
                Clientes = Contexto.Clientes
                .Where(x => x.UltimaData > minimo)
                .Include(x => x.endereco)
                .ToList(),
                Motoristas = Contexto.Motoristas
                .Where(x => x.UltimaData > minimo)
                .ToList(),
                Produtos = Contexto.Produtos
                .Where(x => x.UltimaData > minimo)
                .ToList()
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
