using Microsoft.EntityFrameworkCore;
using BaseGeral.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseGeral.Repositorio
{
    public sealed class Leitura : IDisposable
    {
        AplicativoContext db = new AplicativoContext();

        public void Dispose() => db.Dispose();

        public IEnumerable<(EmitenteDI, byte[])> ObterEmitentes()
        {
            var emitentes = db.Emitentes.ToArray();
            var imagens = db.Imagens;
            var quantEmitentes = emitentes.Length;
            for (int i = 0; i < quantEmitentes; i++)
            {
                var atual = emitentes[i];
                var img = imagens.Find(atual.Id);
                yield return (atual, img?.Bytes);
            }
        }

        public IEnumerable<(Vendedor, byte[])> ObterVendedores()
        {
            var vendedores = db.Vendedores.Where(x => x.Ativo).ToArray();
            var imagens = db.Imagens;
            var quantVendedores = vendedores.Length;
            for (int i = 0; i < quantVendedores; i++)
            {
                var atual = vendedores[i];
                var img = imagens.Find(atual.Id);
                yield return (atual, img?.Bytes);
            }
        }

        public bool EmitentesCadastrados => db.Emitentes.Count() > 0;

        public Imagem ProcurarImagem(Guid id)
        {
            return db.Imagens.Find(id);
        }

        public IEnumerable<ClienteDI> ObterClientes(Func<ClienteDI, bool> expression = null)
        {
            if (expression == null)
            {
                return from cli in db.Clientes
                       where cli.Ativo
                       select cli;
            }
            else
            {
                return from cli in db.Clientes
                       where cli.Ativo && expression(cli)
                       select cli;
            }
        }

        public IEnumerable<VeiculoDI> ObterVeiculos() => db.Veiculos;

        public IEnumerable<ProdutoDI> ObterProdutos()
        {
            return db.Produtos.Where(x => x.Ativo).OrderBy(x => x.Descricao);
        }

        public Estoque ObterEstoque(Guid id)
        {
            return db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<(string, Comprador)> ObterCompradores()
        {
            var original = db.Compradores.Where(x => x.Ativo).OrderBy(x => x.Nome).ToArray();
            for (int i = 0; i < original.Length; i++)
            {
                yield return (db.Clientes.Find(original[i].IdEmpresa).Nome, original[i]);
            }
        }

        public IEnumerable<MotoristaDI> ObterMotoristas()
        {
            return db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome);
        }

        public int ObterMaiorNumeroNFe(string cnpj, ushort serie, bool homologacao)
        {
            return ObterMaiorNumeroFiscal(cnpj, serie, homologacao, false);
        }

        public int ObterMaiorNumeroNFCe(string cnpj, ushort serie, bool homologacao)
        {
            return ObterMaiorNumeroFiscal(cnpj, serie, homologacao, true);
        }

        int ObterMaiorNumeroFiscal(string cnpj, ushort serie, bool homologacao, bool isNFCe)
        {
            var numeros = from nota in db.NotasFiscais
                          where nota.CNPJEmitente == cnpj && nota.IsNFCe == isNFCe
                          where nota.SerieNota == serie
                          let notaHomologacao = nota.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                          where homologacao ? notaHomologacao : !notaHomologacao
                          select nota.NumeroNota;
            return numeros.Count() == 0 ? 0 : numeros.Max();
        }

        public (IEnumerable<NFeDI> emitidas, IEnumerable<NFeDI> outras, IEnumerable<NFeDI> canceladas) ObterNotas(string cnpj, bool isNFCe)
        {
            var notasFiscais = db.NotasFiscais.ToArray();
            var notasEmitidas = (from nota in notasFiscais
                                 where nota.Status == (int)StatusNota.Emitida
                                 where nota.CNPJEmitente == cnpj && nota.IsNFCe == isNFCe
                                 orderby nota.DataEmissao descending
                                 select nota);
            var outrasNotas = (from nota in notasFiscais
                               where nota.Status != (int)StatusNota.Emitida && nota.Status != (int)StatusNota.Cancelada
                               where nota.CNPJEmitente == cnpj && nota.IsNFCe == isNFCe
                               orderby nota.DataEmissao descending
                               select nota);
            var notasCanceladas = (from nota in notasFiscais
                                   where nota.Status == (int)StatusNota.Cancelada
                                   where nota.CNPJEmitente == cnpj && nota.IsNFCe == isNFCe
                                   orderby nota.DataEmissao descending
                                   select nota);
            return (notasEmitidas, outrasNotas, notasCanceladas);
        }

        public Dictionary<Guid, Comprador[]> ObterCompradoresPorCliente()
        {
            return (from cli in db.Clientes.Where(x => x.Ativo).OrderBy(x => x.Nome)
                    join comprador in db.Compradores on cli.Id equals comprador.IdEmpresa into compradores
                    select new { Cliente = cli.Id, Compradores = compradores })
                          .ToDictionary(x => x.Cliente, y => y.Compradores.ToArray());
        }

        public NFeDI ObterNota(string id) => db.NotasFiscais.Find(id);
        public ClienteDI ObterCliente(Guid id) => db.Clientes.Find(id);
        public ClienteDI ObterClienteViaCNPJ(string cnpj) => db.Clientes.FirstOrDefault(x => x.CNPJ == cnpj);
        public MotoristaDI ObterMotorista(Guid id) => db.Motoristas.Find(id);
        public Vendedor ObterVendedor(Guid id) => db.Vendedores.Find(id);
        public Comprador ObterComprador(Guid id) => db.Compradores.Find(id);
        public ProdutoDI ObterProduto(Guid id) => db.Produtos.Find(id);
        public ProdutoDI ObterProduto(string id) => db.Produtos.FirstOrDefault(x => x.CodigoProduto == id);
        public CancelamentoRegistroVenda ObterCRV(Guid id) => db.CancelamentosRegistroVenda.Find(id);

        public IEnumerable<(MotoristaDI, VeiculoDI, VeiculoDI[])> ObterMotoristasComVeiculos()
        {
            foreach (var item1 in db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome).ToArray())
            {
                VeiculoDI item2;
                VeiculoDI[] item3 = null;

                var secs = item1.VeiculosSecundarios;
                if (!string.IsNullOrEmpty(secs))
                {
                    var placas = secs.Split('&');
                    var veics = new VeiculoDI[placas.Length - 1];
                    for (int k = 0; k < veics.Length; k++)
                    {
                        veics[k] = db.Veiculos.First(x => x.Placa == placas[k]);
                    }
                    item3 = veics;
                }
                item2 = db.Veiculos.Find(item1.Veiculo);
                yield return (item1, item2, item3);
            }
        }

        public IEnumerable<(RegistroVenda rv, string vendedor, string cliente, string momento)> ObterRegistrosVenda(Guid emitente)
        {
            return from venda in db.Vendas.Include(x => x.Produtos).ToArray()
                   where venda.Emitente == emitente
                   orderby venda.DataHoraVenda descending
                   select (venda,
                       venda.Vendedor != default(Guid) ? db.Vendedores.Find(venda.Vendedor).Nome : "Indisponível",
                       venda.Cliente != default(Guid) ? db.Clientes.Find(venda.Cliente).Nome : "Indisponível",
                       venda.DataHoraVenda.ToString("HH:mm:ss dd-MM-yyyy"));
        }

        public IEnumerable<Estoque> ObterEstoques() => db.Estoque.Include(x => x.Alteracoes);

        public IEnumerable<int> ObterAnosNotas(string cnpjEmitente, bool isNFCe)
        {
            return (from dado in db.NotasFiscais
                    let notaHomologacao = dado.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                    where dado.Status == (int)StatusNota.Emitida && !notaHomologacao && dado.IsNFCe == isNFCe
                    where dado.CNPJEmitente == cnpjEmitente
                    let ano = DateTime.Parse(dado.DataEmissao).Year
                    orderby ano ascending
                    select ano).Distinct();
        }

        public Dictionary<int, IEnumerable<(DateTime, string)>> ObterNFesPorAno(string cnpjEmitente, bool isNFCe)
        {
            return (from item in db.NotasFiscais
                    let notaHomologacao = item.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                    where item.Status == (int)StatusNota.Emitida && !notaHomologacao && item.IsNFCe == isNFCe
                    where item.CNPJEmitente == cnpjEmitente
                    let data = DateTime.Parse(item.DataEmissao)
                    group new { Data = data, item.XML } by data.Year).ToDictionary(x => x.Key, x => x.Select(k => (k.Data, k.XML)));
        }

        public IEnumerable<Inutilizacao> ObterInutilizacoes(bool isNFCe) => db.Inutilizacoes.Where(x => x.CNPJ == DefinicoesTemporarias.EmitenteAtivo.CNPJ && x.IsNFCe == isNFCe);
    }
}
