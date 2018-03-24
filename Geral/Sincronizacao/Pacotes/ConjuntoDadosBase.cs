using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public sealed class ConjuntoDadosBase
    {
        public ClienteDI[] Clientes { get; set; }
        public EmitenteDI[] Emitentes { get; set; }
        public MotoristaDI[] Motoristas { get; set; }
        public Vendedor[] Vendedores { get; set; }
        public ProdutoDI[] Produtos { get; set; }
        public Estoque[] Estoque { get; set; }
        public VeiculoDI[] Veiculos { get; set; }
        public RegistroVenda[] Vendas { get; set; }
        public RegistroCancelamento[] Cancelamentos { get; set; }
        public CancelamentoRegistroVenda[] CancelamentosRegistroVenda { get; set; }
        public Imagem[] Imagens { get; set; }
        public Comprador[] Compradores { get; set; }
        public Inutilizacao[] Inutilizacoes { get; set; }

        public DateTime InstanteSincronizacao { get; set; }

        public ConjuntoDadosBase() { }

        public ConjuntoDadosBase(DateTime minimo)
        {
            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.Where(x => x.UltimaData > minimo).ToArray();
                Emitentes = db.Emitentes.Where(x => x.UltimaData > minimo).ToArray();
                Motoristas = db.Motoristas.Where(x => x.UltimaData > minimo).ToArray();
                Vendedores = db.Vendedores.Where(x => x.UltimaData > minimo).ToArray();
                Produtos = db.Produtos.Where(x => x.UltimaData > minimo).ToArray();
                Estoque = db.Estoque.Include(x => x.Alteracoes).Where(x => x.UltimaData > minimo).ToArray();
                Veiculos = db.Veiculos.ToArray();
                Vendas = db.Vendas.Include(x => x.Produtos).Where(x => x.UltimaData > minimo).ToArray();
                Cancelamentos = db.Cancelamentos.ToArray();
                CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToArray();
                Imagens = db.Imagens.Where(x => x.UltimaData > minimo).ToArray();
                Compradores = db.Compradores.Where(x => x.UltimaData > minimo).ToArray();
                Inutilizacoes = db.Inutilizacoes.ToArray();
            }
        }

        public ConjuntoDadosBase(ConjuntoDadosBase existente, DateTime minimo, DateTime atual)
        {
            InstanteSincronizacao = atual;
            using (var db = new AplicativoContext())
            {
                Clientes = (from local in db.Clientes
                            let servidor = existente.Clientes.FirstOrDefault(x => x.Id == local.Id)
                            where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                            select local).ToArray();

                Emitentes = (from local in db.Emitentes
                             let servidor = existente.Emitentes.FirstOrDefault(x => x.Id == local.Id)
                             where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                             select local).ToArray();

                Motoristas = (from local in db.Motoristas
                              let servidor = existente.Motoristas.FirstOrDefault(x => x.Id == local.Id)
                              where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                              select local).ToArray();

                Vendedores = (from local in db.Vendedores
                              let servidor = existente.Vendedores.FirstOrDefault(x => x.Id == local.Id)
                              where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                              select local).ToArray();

                Produtos = (from local in db.Produtos
                            let servidor = existente.Produtos.FirstOrDefault(x => x.Id == local.Id)
                            where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                            select local).ToArray();

                Estoque = (from local in db.Estoque.Include(x => x.Alteracoes)
                           let servidor = existente.Estoque.FirstOrDefault(x => x.Id == local.Id)
                           where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                           select local).ToArray();

                Veiculos = (from local in db.Veiculos
                            let servidor = existente.Veiculos.FirstOrDefault(x => x.Id == local.Id)
                            where servidor == null
                            select local).ToArray();

                Vendas = (from local in db.Vendas.Include(x => x.Produtos)
                          let servidor = existente.Vendas.FirstOrDefault(x => x.Id == local.Id)
                          where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                          select local).ToArray();

                Cancelamentos = (from local in db.Cancelamentos
                                 where !existente.Cancelamentos.Any(x => x.ChaveNFe == local.ChaveNFe)
                                 select local).ToArray();

                CancelamentosRegistroVenda = (from local in db.CancelamentosRegistroVenda
                                              where !existente.CancelamentosRegistroVenda.Any(x => x.Id == local.Id)
                                              select local).ToArray();

                Imagens = (from local in db.Imagens
                           let servidor = existente.Imagens.FirstOrDefault(x => x.Id == local.Id)
                           where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                           select local).ToArray();

                Compradores = (from local in db.Compradores
                               let servidor = existente.Compradores.FirstOrDefault(x => x.Id == local.Id)
                               where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                               select local).ToArray();

                Inutilizacoes = (from local in db.Inutilizacoes
                                 where !existente.Inutilizacoes.Any(x => x.Id == local.Id)
                                 select local).ToArray();
            }
        }

        public void AnalisarESalvar(DateTime minimo)
        {
            List<AlteracaoEstoque>[] AlteracoesEstoque = null;
            List<ProdutoSimplesVenda>[] ProdutosVendas = null;

            using (var db = new AplicativoContext())
            {
                List<object> Adicionar = new List<object>();
                List<object> Atualizar = new List<object>();

                if (Clientes != null)
                {
                    for (int i = 0; i < Clientes.Length; i++)
                    {
                        var novo = Clientes[i];
                        var atual = db.Clientes.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Emitentes != null)
                {
                    for (int i = 0; i < Emitentes.Length; i++)
                    {
                        var novo = Emitentes[i];
                        var atual = db.Emitentes.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Motoristas != null)
                {
                    for (int i = 0; i < Motoristas.Length; i++)
                    {
                        var novo = Motoristas[i];
                        var atual = db.Motoristas.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Vendedores != null)
                {
                    for (int i = 0; i < Vendedores.Length; i++)
                    {
                        var novo = Vendedores[i];
                        var atual = db.Vendedores.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Produtos != null)
                {
                    for (int i = 0; i < Produtos.Length; i++)
                    {
                        var novo = Produtos[i];
                        var atual = db.Produtos.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Veiculos != null)
                {
                    for (int i = 0; i < Veiculos.Length; i++)
                    {
                        var novo = Veiculos[i];
                        var atual = db.Veiculos.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            Adicionar.Add(novo);
                        }
                    }
                }

                if (Cancelamentos != null)
                {
                    for (int i = 0; i < Cancelamentos.Length; i++)
                    {
                        var novo = Cancelamentos[i];
                        var atual = db.Cancelamentos.FirstOrDefault(x => x.ChaveNFe == novo.ChaveNFe);
                        if (atual == null)
                        {
                            Adicionar.Add(novo);
                        }
                    }
                }

                if (CancelamentosRegistroVenda != null)
                {
                    for (int i = 0; i < CancelamentosRegistroVenda.Length; i++)
                    {
                        var novo = CancelamentosRegistroVenda[i];
                        var atual = db.CancelamentosRegistroVenda.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            Adicionar.Add(novo);
                        }
                    }
                }

                if (Inutilizacoes != null)
                {
                    for (int i = 0; i < Inutilizacoes.Length; i++)
                    {
                        var novo = Inutilizacoes[i];
                        var atual = db.Inutilizacoes.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            Adicionar.Add(novo);
                        }
                    }
                }

                if (Imagens != null)
                {
                    for (int i = 0; i < Imagens.Length; i++)
                    {
                        var novo = Imagens[i];
                        var atual = db.Imagens.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Compradores != null)
                {
                    for (int i = 0; i < Compradores.Length; i++)
                    {
                        var novo = Compradores[i];
                        var atual = db.Compradores.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Estoque != null)
                {
                    AlteracoesEstoque = new List<AlteracaoEstoque>[Estoque.Length];
                    for (int i = 0; i < Estoque.Length; i++)
                    {
                        var novo = Estoque[i];

                        AlteracoesEstoque[i] = novo.Alteracoes;
                        novo.Alteracoes = null;

                        var atual = db.Estoque.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                if (Vendas != null)
                {
                    ProdutosVendas = new List<ProdutoSimplesVenda>[Vendas.Length];
                    for (int i = 0; i < Vendas.Length; i++)
                    {
                        var novo = Vendas[i];

                        ProdutosVendas[i] = novo.Produtos;
                        novo.Produtos = null;

                        var atual = db.Vendas.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                db.AddRange(Adicionar);
                db.UpdateRange(Atualizar);
                db.SaveChanges();
            }

            using (var db = new AplicativoContext())
            {
                if (AlteracoesEstoque != null)
                {
                    for (int i = 0; i < Estoque.Length; i++)
                    {
                        var novo = Estoque[i];
                        var alteracoes = AlteracoesEstoque[i];
                        alteracoes.ForEach(x => x.Id = default(Guid));
                        var original = db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == novo.Id);
                        var maiorData = original.Alteracoes.Count > 0 ? original.Alteracoes.Max(k => k.MomentoRegistro) : DateTime.MinValue;
                        novo.Alteracoes = alteracoes.Where(x => x.MomentoRegistro > maiorData).ToList();
                        db.Estoque.Update(novo);
                    }
                }

                if (ProdutosVendas != null)
                {
                    for (int i = 0; i < Vendas.Length; i++)
                    {
                        var novo = Vendas[i];
                        var produtos = ProdutosVendas[i];
                        produtos.ForEach(x => x.Id = default(Guid));
                        novo.Produtos = produtos.ToList();
                        db.Vendas.Update(novo);
                    }
                }

                db.SaveChanges();
            }
        }

        public void AtualizarPadrao()
        {
            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.ToArray();
                Emitentes = db.Emitentes.ToArray();
                Motoristas = db.Motoristas.ToArray();
                Vendedores = db.Vendedores.ToArray();
                Produtos = db.Produtos.ToArray();
                Estoque = db.Estoque.Include(x => x.Alteracoes).ToArray();
                Veiculos = db.Veiculos.ToArray();
                Vendas = db.Vendas.Include(x => x.Produtos).ToArray();
                Cancelamentos = db.Cancelamentos.ToArray();
                CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToArray();
                Imagens = db.Imagens.ToArray();
                Compradores = db.Compradores.ToArray();
                Inutilizacoes = db.Inutilizacoes.ToArray();
            }
        }
    }
}
