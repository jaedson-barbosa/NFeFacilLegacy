using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Certificacao.LAN.PacotesBanco
{
    public struct ConjuntoBanco
    {
        public ClienteDI[] Clientes { get; set; }
        public EmitenteDI[] Emitentes { get; set; }
        public MotoristaDI[] Motoristas { get; set; }
        public Vendedor[] Vendedores { get; set; }
        public ProdutoDI[] Produtos { get; set; }
        public Estoque[] Estoque { get; set; }
        public VeiculoDI[] Veiculos { get; set; }
        public NFeDI[] NotasFiscais { get; set; }
        public RegistroVenda[] Vendas { get; set; }
        public RegistroCancelamento[] Cancelamentos { get; set; }
        public CancelamentoRegistroVenda[] CancelamentosRegistroVenda { get; set; }
        public Imagem[] Imagens { get; set; }

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
                NotasFiscais = db.NotasFiscais.ToArray();
                Vendas = db.Vendas.Include(x => x.Produtos).ToArray();
                Cancelamentos = db.Cancelamentos.ToArray();
                CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToArray();
                Imagens = db.Imagens.ToArray();
            }
        }

        public void AnalisarESalvar()
        {
            List<AlteracaoEstoque>[] AlteracoesEstoque = null;
            List<ProdutoSimplesVenda>[] ProdutosVendas = null;

            using (var db = new AplicativoContext())
            {
                if (Clientes.Length > 0) db.AddRange(Clientes);
                if (Emitentes.Length > 0) db.AddRange(Emitentes);
                if (Motoristas.Length > 0) db.AddRange(Motoristas);
                if (Vendedores.Length > 0) db.AddRange(Vendedores);
                if (Produtos.Length > 0) db.AddRange(Produtos);
                if (Veiculos.Length > 0) db.AddRange(Veiculos);
                if (NotasFiscais.Length > 0) db.AddRange(NotasFiscais);
                if (Cancelamentos.Length > 0) db.AddRange(Cancelamentos);
                if (CancelamentosRegistroVenda.Length > 0) db.AddRange(CancelamentosRegistroVenda);
                if (Imagens.Length > 0) db.AddRange(Imagens);
                db.SaveChanges();

                if (Estoque.Length > 0)
                {
                    AlteracoesEstoque = new List<AlteracaoEstoque>[Estoque.Length];
                    for (int i = 0; i < Estoque.Length; i++)
                    {
                        var novo = Estoque[i];
                        AlteracoesEstoque[i] = novo.Alteracoes;
                        novo.Alteracoes = null;
                        db.Add(novo);
                    }
                }

                if (Vendas.Length > 0)
                {
                    ProdutosVendas = new List<ProdutoSimplesVenda>[Vendas.Length];
                    for (int i = 0; i < Vendas.Length; i++)
                    {
                        var novo = Vendas[i];
                        ProdutosVendas[i] = novo.Produtos;
                        novo.Produtos = null;
                        db.Add(novo);
                    }
                }
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
    }
}
