using NFeFacil.ItensBD;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public sealed class ConjuntoBanco : IPacote
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
            Estoque = db.Estoque.ToList();
            Veiculos = db.Veiculos.ToList();
            NotasFiscais = db.NotasFiscais.ToList();
            Vendas = db.Vendas.ToList();
            Cancelamentos = db.Cancelamentos.ToList();
            CancelamentosRegistroVenda = db.CancelamentosRegistroVenda.ToList();
            Imagens = db.Imagens.ToList();
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

            Estoque = (from local in db.Estoque
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

            Vendas = (from local in db.Vendas
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
    }
}
