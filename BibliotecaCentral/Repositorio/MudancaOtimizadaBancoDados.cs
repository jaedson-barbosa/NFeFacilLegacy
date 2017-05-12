using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.Repositorio
{
    internal sealed class MudancaOtimizadaBancoDados
    {
        private AplicativoContext Contexto { get; }
        internal MudancaOtimizadaBancoDados(AplicativoContext contexto)
        {
            Contexto = contexto;
            Contexto.ChangeTracker.AutoDetectChangesEnabled = false;
            Contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        internal void AdicionarEmitentes(List<Emitente> emitentes)
        {
            emitentes.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) != null);
            var naoExistem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarEmitentes(List<Emitente> emitentes)
        {
            var existem = new List<Emitente>();
            var naoExistem = new List<Emitente>();
            foreach (var emit in emitentes)
            {
                if (emit.Id != null && Contexto.Emitentes.Find(emit.Id) != null)
                {
                    existem.Add(emit);
                }
                else
                {
                    var busca = Contexto.Emitentes.FirstOrDefault(x => x.CNPJ == emit.CNPJ);
                    if (busca != default(Emitente))
                    {
                        emit.Id = busca.Id;
                        existem.Add(emit);
                    }
                    else
                    {
                        naoExistem.Add(emit);
                    }
                }
            }
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AdicionarClientes(List<Destinatario> clientes)
        {
            clientes.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = clientes.FindAll(x => Contexto.Clientes.Find(x.Id) != null);
            var naoExistem = clientes.FindAll(x => Contexto.Clientes.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarClientes(List<Destinatario> clientes)
        {
            var existem = new List<Destinatario>();
            var naoExistem = new List<Destinatario>();
            foreach (var dest in clientes)
            {
                if (dest.Id != null && Contexto.Clientes.Find(dest.Id) != null)
                {
                    existem.Add(dest);
                }
                else
                {
                    var busca = Contexto.Clientes.FirstOrDefault(x => x.Documento == dest.Documento);
                    if (busca != default(Destinatario))
                    {
                        dest.Id = busca.Id;
                        existem.Add(dest);
                    }
                    else
                    {
                        naoExistem.Add(dest);
                    }
                }
            }
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AdicionarMotoristas(List<Motorista> motoristas)
        {
            motoristas.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) != null);
            var naoExistem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarMotoristas(List<Motorista> motoristas)
        {
            var existem = new List<Motorista>();
            var naoExistem = new List<Motorista>();
            foreach (var mot in motoristas)
            {
                if (mot.Id != null && Contexto.Motoristas.Find(mot.Id) != null)
                {
                    existem.Add(mot);
                }
                else
                {
                    var busca = Contexto.Motoristas.FirstOrDefault(x => x.Documento == mot.Documento
                        || x.InscricaoEstadual == mot.InscricaoEstadual
                        || (x.Nome == mot.Nome && x.XEnder == mot.XEnder));
                    if (busca != default(Motorista))
                    {
                        mot.Id = busca.Id;
                        existem.Add(mot);
                    }
                    else
                    {
                        naoExistem.Add(mot);
                    }
                }
            }
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AdicionarProdutos(List<BaseProdutoOuServico> produtos)
        {
            produtos.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = produtos.FindAll(x => Contexto.Produtos.Find(x.Id) != null);
            var naoExistem = produtos.FindAll(x => Contexto.Produtos.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarProdutos(List<BaseProdutoOuServico> produtos)
        {
            var existem = new List<BaseProdutoOuServico>();
            var naoExistem = new List<BaseProdutoOuServico>();
            foreach (var prod in produtos)
            {
                if (prod.Id != null && Contexto.Produtos.Find(prod.Id) != null)
                {
                    existem.Add(prod);
                }
                else
                {
                    var busca = Contexto.Produtos.FirstOrDefault(x => x.Descricao == prod.Descricao
                        || (x.CodigoProduto == prod.CodigoProduto && x.CFOP == prod.CFOP));
                    if (busca != default(BaseProdutoOuServico))
                    {
                        prod.Id = busca.Id;
                        existem.Add(prod);
                    }
                    else
                    {
                        naoExistem.Add(prod);
                    }
                }
            }
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal async Task AdicionarNotasFiscais(Dictionary<NFeDI, XElement> notas)
        {
            var pasta = new PastaNotasFiscais();
            foreach (var item in notas)
            {
                item.Key.UltimaData = DateTime.Now;
                if (Contexto.NotasFiscais.Count(x => x.Id == item.Key.Id) > 0)
                {
                    Contexto.Update(item.Key);
                }
                else
                {
                    Contexto.Add(item.Key);
                }
                await pasta.AdicionarOuAtualizar(item.Value, item.Key.Id);
            }
        }
    }
}
