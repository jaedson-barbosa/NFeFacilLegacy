using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Repositorio
{
    internal sealed class MudancaOtimizadaBancoDados
    {
        private AplicativoContext Contexto { get; }
        internal MudancaOtimizadaBancoDados(AplicativoContext contexto)
        {
            Contexto = contexto;
        }

        internal void AdicionarEmitentes(List<EmitenteDI> emitentes)
        {
            emitentes.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) != null);
            var naoExistem = emitentes.FindAll(x => Contexto.Emitentes.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarEmitentes(List<EmitenteDI> emitentes)
        {
            var existem = new List<EmitenteDI>();
            var naoExistem = new List<EmitenteDI>();
            foreach (var emit in emitentes)
            {
                if (emit.Id != null && Contexto.Emitentes.Find(emit.Id) != null)
                {
                    existem.Add(emit);
                }
                else
                {
                    var busca = Contexto.Emitentes.FirstOrDefault(x => x.CNPJ == emit.CNPJ);
                    if (busca != default(EmitenteDI))
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

        internal void AdicionarClientes(List<ClienteDI> clientes)
        {
            clientes.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = clientes.FindAll(x => Contexto.Clientes.Find(x.Id) != null);
            var naoExistem = clientes.FindAll(x => Contexto.Clientes.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarClientes(List<ClienteDI> clientes)
        {
            var existem = new List<ClienteDI>();
            var naoExistem = new List<ClienteDI>();
            foreach (var dest in clientes)
            {
                if (dest.Id != null && Contexto.Clientes.Find(dest.Id) != null)
                {
                    existem.Add(dest);
                }
                else
                {
                    var busca = Contexto.Clientes.FirstOrDefault(x => x.Documento == dest.Documento);
                    if (busca != default(ClienteDI))
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

        internal void AdicionarMotoristas(List<MotoristaDI> motoristas)
        {
            motoristas.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) != null);
            var naoExistem = motoristas.FindAll(x => Contexto.Motoristas.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarMotoristas(List<MotoristaDI> motoristas)
        {
            var existem = new List<MotoristaDI>();
            var naoExistem = new List<MotoristaDI>();
            foreach (var mot in motoristas)
            {
                if (mot.Id != null && Contexto.Motoristas.Find(mot.Id) != null)
                {
                    existem.Add(mot);
                }
                else
                {
                    var busca = Contexto.Motoristas.FirstOrDefault(x => x.Documento == mot.Documento
                        || (x.Nome == mot.Nome && x.XEnder == mot.XEnder));
                    if (busca != default(MotoristaDI))
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

        internal void AdicionarProdutos(List<ProdutoDI> produtos)
        {
            produtos.ForEach(x => x.UltimaData = DateTime.Now);
            var existem = produtos.FindAll(x => Contexto.Produtos.Find(x.Id) != null);
            var naoExistem = produtos.FindAll(x => Contexto.Produtos.Find(x.Id) == null);
            Contexto.AddRange(naoExistem);
            Contexto.UpdateRange(existem);
        }

        internal void AnalisarAdicionarProdutos(List<ProdutoDI> produtos)
        {
            var existem = new List<ProdutoDI>();
            var naoExistem = new List<ProdutoDI>();
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
                    if (busca != default(ProdutoDI))
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

        internal void AdicionarNotasFiscais(List<NFeDI> notas)
        {
            foreach (var item in notas)
            {
                item.UltimaData = DateTime.Now;
                if (Contexto.NotasFiscais.Count(x => x.Id == item.Id) > 0)
                {
                    Contexto.Update(item);
                }
                else
                {
                    Contexto.Add(item);
                }
            }
        }
    }
}
