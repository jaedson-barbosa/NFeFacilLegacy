using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil
{
    internal sealed class MudancaOtimizadaBancoDados
    {
        private AplicativoContext Contexto { get; }
        internal MudancaOtimizadaBancoDados(AplicativoContext contexto)
        {
            Contexto = contexto;
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
            existem.ForEach(x => x.UltimaData = DateTime.Now);
            naoExistem.ForEach(x => x.UltimaData = DateTime.Now);
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
            existem.ForEach(x => x.UltimaData = DateTime.Now);
            naoExistem.ForEach(x => x.UltimaData = DateTime.Now);
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
            existem.ForEach(x => x.UltimaData = DateTime.Now);
            naoExistem.ForEach(x => x.UltimaData = DateTime.Now);
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
