using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
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

        private void AdicionarEmitentes(IEnumerable<Emitente> emitentes)
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

        private void AdicionarClientes(IEnumerable<Destinatario> clientes)
        {
            var analise = from cli in clientes
                          group cli by Contexto.Clientes.Count(x => x.Documento == cli.Documento) == 0;
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
                        Contexto.Update(cli);
                    }
                }
            }
        }

        private void AdicionarMotoristas(IEnumerable<Motorista> motoristas)
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
                        Contexto.Update(mot);
                    }
                }
            }
        }

        private void AdicionarProdutos(IEnumerable<BaseProdutoOuServico> produtos)
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
                        Contexto.Update(prod);
                    }
                }
            }
        }

        public void Dispose() => Contexto.Dispose();
    }
}
