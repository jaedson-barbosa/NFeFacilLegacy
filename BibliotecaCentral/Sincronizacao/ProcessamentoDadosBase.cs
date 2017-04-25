using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Emitentes = Contexto.Emitentes.Include(x => x.endereco),
                Clientes = Contexto.Clientes.Include(x => x.endereco),
                Motoristas = Contexto.Motoristas,
                Produtos = Contexto.Produtos
            };
        }

        public async Task SalvarAsync(DadosBase dados)
        {
            AdicionarEmitentes(dados.Emitentes);
            AdicionarClientes(dados.Clientes);
            AdicionarMotoristas(dados.Motoristas); ;
            AdicionarProdutos(dados.Produtos);
            await Contexto.SaveChangesAsync();
        }

        private void AdicionarEmitentes(IEnumerable<EmitenteDI> emitentes)
        {
            var analise = from emit in emitentes
                          group emit by Contexto.Emitentes.Count(x => x.CNPJ == emit.CNPJ) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    foreach (var emit in item)
                    {
                        try
                        {
                            emit.Id = AcharId(emit.CNPJ);
                            Contexto.Update(emit);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("Deu errado alguma coisa.");
                        }
                    }
                }
            }
        }

        private int AcharId(string CNPJ)
        {
            return Contexto.Emitentes.Include(x => x.endereco).First(x => x.CNPJ == CNPJ).Id;
        }

        private void AdicionarClientes(IEnumerable<ClienteDI> clientes)
        {
            var analise = from cli in clientes
                          group cli by Contexto.Clientes.Count(x => x.obterDocumento == cli.obterDocumento) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    foreach (var cli in item)
                    {
                        cli.Id = Contexto.Clientes.First(x => x.obterDocumento == cli.obterDocumento).Id;
                        Contexto.Update(cli);
                    }
                }
            }
        }

        private void AdicionarMotoristas(IEnumerable<MotoristaDI> motoristas)
        {
            var analise = from mot in motoristas
                          group mot by Contexto.Motoristas.Count(x => x.Documento == mot.Documento) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    foreach (var mot in item)
                    {
                        mot.Id = Contexto.Motoristas.First(x => x.Documento == mot.Documento).Id;
                        Contexto.Update(mot);
                    }
                }
            }
        }

        private void AdicionarProdutos(IEnumerable<ProdutoDI> produtos)
        {
            var analise = from prod in produtos
                          group prod by Contexto.Produtos.Count(x => x.Descricao == prod.Descricao) == 0;
            foreach (var item in analise)
            {
                if (item.Key)
                {
                    Contexto.AddRange(item);
                }
                else
                {
                    foreach (var prod in item)
                    {
                        prod.Id = Contexto.Produtos.First(x => x.Descricao == prod.Descricao).Id;
                        Contexto.Update(prod);
                    }
                }
            }
        }

        public void Dispose() => Contexto.Dispose();
    }
}
