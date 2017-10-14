using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Backup
{
    struct ConjuntoBanco
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

        public void AtualizarPadrao()
        {
            using (var db = new AplicativoContext())
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
        }

        public void AnalisarESalvar()
        {
            using (var db = new AplicativoContext())
            {
                db.AddRange(Clientes);
                db.AddRange(Emitentes);
                db.AddRange(Motoristas);
                db.AddRange(Vendedores);
                db.AddRange(Produtos);
                db.AddRange(Estoque);
                db.AddRange(Veiculos);
                db.AddRange(NotasFiscais);
                db.AddRange(Vendas);
                db.AddRange(Cancelamentos);
                db.AddRange(CancelamentosRegistroVenda);
                db.AddRange(Imagens);
                db.SaveChanges();
            }
        }
    }
}
