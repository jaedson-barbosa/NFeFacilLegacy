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
    internal static class ProcessamentoDadosBase
    {
        public static DadosBase Obter()
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

        public async static Task SalvarAsync(DadosBase dados)
        {
            using (var Contexto = new AplicativoContext())
            {
                AdicionarEmitentes(dados.Emitentes);
                AdicionarClientes(dados.Clientes);
                AdicionarMotoristas(dados.Motoristas); ;
                AdicionarProdutos(dados.Produtos);
                await Contexto.SaveChangesAsync();

                void AdicionarEmitentes(IEnumerable<Emitente> emitentes)
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
                            Contexto.UpdateRange(item);
                        }
                    }
                }

                void AdicionarClientes(IEnumerable<Destinatario> clientes)
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
                            Contexto.UpdateRange(item);
                        }
                    }
                }

                void AdicionarMotoristas(IEnumerable<Motorista> motoristas)
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
                            Contexto.UpdateRange(item);
                        }
                    }
                }

                void AdicionarProdutos(IEnumerable<BaseProdutoOuServico> produtos)
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
                            Contexto.UpdateRange(item);
                        }
                    }
                }
            }
        }
    }
}
