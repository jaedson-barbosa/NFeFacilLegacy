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
    internal sealed class MudancaOtimizadaBancoDados : ConexaoBanco
    {
        internal MudancaOtimizadaBancoDados() : base()
        {
            Contexto.ChangeTracker.AutoDetectChangesEnabled = false;
            Contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        internal MudancaOtimizadaBancoDados(AplicativoContext contexto) : base(contexto) { }

        internal void AdicionarEmitentes(List<Emitente> emitentes)
        {
            emitentes.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) != null);
            var naoExistem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) == null);
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

        internal void AdicionarMotoristas(List<Motorista> motoristas)
        {
            motoristas.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) != null);
            var naoExistem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) == null);
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

        internal void AdicionarProdutosAnaliseCompleta(List<BaseProdutoOuServico> produtos)
        {
            for (int i = 0; i < produtos.Count; i++)
            {

            }
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
