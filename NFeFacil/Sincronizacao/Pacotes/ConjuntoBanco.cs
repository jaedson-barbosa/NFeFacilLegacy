using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public struct ConjuntoBanco : IPacote
    {
        public List<ClienteDI> Clientes { get; set; }
        public List<EmitenteDI> Emitentes { get; set; }
        public List<MotoristaDI> Motoristas { get; set; }
        public List<Vendedor> Vendedores { get; set; }
        public List<ProdutoDI> Produtos { get; set; }
        public List<Estoque> Estoque { get; set; }
        public List<VeiculoDI> Veiculos { get; set; }
        public List<NFeDI> NotasFiscais { get; set; }
        public List<RegistroVenda> Vendas { get; set; }
        public List<RegistroCancelamento> Cancelamentos { get; set; }
        public List<CancelamentoRegistroVenda> CancelamentosRegistroVenda { get; set; }
        public List<Imagem> Imagens { get; set; }

        public ConjuntoBanco(AplicativoContext db)
        {
            Clientes = db.Clientes.ToList();
            Emitentes = db.Emitentes.ToList();
            Motoristas = db.Motoristas.ToList();
            Vendedores = db.Vendedores.ToList();
            Produtos = db.Produtos.ToList();
            Estoque = db.Estoque.Include(x => x.Alteracoes).ToList();
            Veiculos = db.Veiculos.ToList();
            NotasFiscais = db.NotasFiscais.ToList();
            Vendas = db.Vendas.Include(x => x.Produtos).ToList();
            Cancelamentos = db.Cancelamentos.ToList();
            CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToList();
            Imagens = db.Imagens.ToList();
        }

        public ConjuntoBanco(AplicativoContext db, DateTime minimo)
        {
            if (minimo.Ticks > 10) minimo = minimo.AddSeconds(-10);

            Clientes = db.Clientes.Where(x => x.UltimaData > minimo).ToList();
            Emitentes = db.Emitentes.Where(x => x.UltimaData > minimo).ToList();
            Motoristas = db.Motoristas.Where(x => x.UltimaData > minimo).ToList();
            Vendedores = db.Vendedores.Where(x => x.UltimaData > minimo).ToList();
            Produtos = db.Produtos.Where(x => x.UltimaData > minimo).ToList();
            Estoque = db.Estoque.Include(x => x.Alteracoes).Where(x => x.UltimaData > minimo).ToList();
            Veiculos = db.Veiculos.ToList();
            NotasFiscais = db.NotasFiscais.Where(x => x.UltimaData > minimo).ToList();
            Vendas = db.Vendas.Include(x => x.Produtos).ToList();
            Cancelamentos = db.Cancelamentos.ToList();
            CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToList();
            Imagens = db.Imagens.Where(x => x.UltimaData > minimo).ToList();
        }

        public ConjuntoBanco(ConjuntoBanco existente, AplicativoContext db)
        {
            Clientes = (from local in db.Clientes
                        let servidor = existente.Clientes.FirstOrDefault(x => x.Id == local.Id)
                        where servidor == null || servidor.UltimaData < local.UltimaData
                        select local).ToList();

            Emitentes = (from local in db.Emitentes
                         let servidor = existente.Emitentes.FirstOrDefault(x => x.Id == local.Id)
                         where servidor == null || servidor.UltimaData < local.UltimaData
                         select local).ToList();

            Motoristas = (from local in db.Motoristas
                          let servidor = existente.Motoristas.FirstOrDefault(x => x.Id == local.Id)
                          where servidor == null || servidor.UltimaData < local.UltimaData
                          select local).ToList();

            Vendedores = (from local in db.Vendedores
                          let servidor = existente.Vendedores.FirstOrDefault(x => x.Id == local.Id)
                          where servidor == null || servidor.UltimaData < local.UltimaData
                          select local).ToList();

            Produtos = (from local in db.Produtos
                        let servidor = existente.Produtos.FirstOrDefault(x => x.Id == local.Id)
                        where servidor == null || servidor.UltimaData < local.UltimaData
                        select local).ToList();

            Estoque = (from local in db.Estoque.Include(x => x.Alteracoes)
                       let servidor = existente.Estoque.FirstOrDefault(x => x.Id == local.Id)
                       where servidor == null || servidor.UltimaData < local.UltimaData
                       select local).ToList();

            Veiculos = (from local in db.Veiculos
                        let servidor = existente.Veiculos.FirstOrDefault(x => x.Id == local.Id)
                        where servidor == null
                        select local).ToList();

            NotasFiscais = (from local in db.NotasFiscais
                            let servidor = existente.NotasFiscais.FirstOrDefault(x => x.Id == local.Id)
                            where servidor == null || servidor.UltimaData < local.UltimaData
                            select local).ToList();

            Vendas = (from local in db.Vendas.Include(x => x.Produtos)
                      let servidor = existente.Vendas.FirstOrDefault(x => x.Id == local.Id)
                      where servidor == null || (!servidor.Cancelado && local.Cancelado)
                      select local).ToList();

            Cancelamentos = (from local in db.Cancelamentos
                             where existente.Cancelamentos.Count(x => x.ChaveNFe == local.ChaveNFe) == 0
                             select local).ToList();

            CancelamentosRegistroVenda = (from local in db.CancelamentosRegistroVenda
                                          where existente.CancelamentosRegistroVenda.Count(x => x.Id == local.Id) == 0
                                          select local).ToList();

            Imagens = (from local in db.Imagens
                       let servidor = existente.Imagens.FirstOrDefault(x => x.Id == local.Id)
                       where servidor == null || servidor.UltimaData < local.UltimaData
                       select local).ToList();
        }

        public void AnalisarESalvar(AplicativoContext db)
        {
            List<object> Adicionar = new List<object>();
            List<object> Atualizar = new List<object>();

            for (int i = 0; i < Clientes.Count; i++)
            {
                var novo = Clientes[i];
                var atual = db.Clientes.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Emitentes.Count; i++)
            {
                var novo = Emitentes[i];
                var atual = db.Emitentes.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Motoristas.Count; i++)
            {
                var novo = Motoristas[i];
                var atual = db.Motoristas.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Vendedores.Count; i++)
            {
                var novo = Vendedores[i];
                var atual = db.Vendedores.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Produtos.Count; i++)
            {
                var novo = Produtos[i];
                var atual = db.Produtos.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Estoque.Count; i++)
            {
                var novo = Estoque[i];
                var atual = db.Estoque.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Veiculos.Count; i++)
            {
                var novo = Veiculos[i];
                var atual = db.Veiculos.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
            }

            for (int i = 0; i < NotasFiscais.Count; i++)
            {
                var novo = NotasFiscais[i];
                var atual = db.NotasFiscais.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Vendas.Count; i++)
            {
                var novo = Vendas[i];
                var atual = db.Vendas.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.Cancelado && !atual.Cancelado)
                {
                    Atualizar.Add(novo);
                }
            }

            for (int i = 0; i < Cancelamentos.Count; i++)
            {
                var novo = Cancelamentos[i];
                var atual = db.Cancelamentos.FirstOrDefault(x => x.ChaveNFe == novo.ChaveNFe);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
            }

            for (int i = 0; i < CancelamentosRegistroVenda.Count; i++)
            {
                var novo = CancelamentosRegistroVenda[i];
                var atual = db.CancelamentosRegistroVenda.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
            }

            for (int i = 0; i < Imagens.Count; i++)
            {
                var novo = Imagens[i];
                var atual = db.Imagens.FirstOrDefault(x => x.Id == novo.Id);
                if (atual == null)
                {
                    Adicionar.Add(novo);
                }
                else if (novo.UltimaData > atual.UltimaData)
                {
                    Atualizar.Add(novo);
                }
            }

            db.AddRange(Adicionar);
            db.UpdateRange(Atualizar);
        }
    }
}
