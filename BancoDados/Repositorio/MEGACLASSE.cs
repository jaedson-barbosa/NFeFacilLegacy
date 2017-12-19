using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeFacil.Repositorio
{
    public class MEGACLASSE : IDisposable
    {
        AplicativoContext db = new AplicativoContext();

        public void Dispose()
        {
            db.SaveChanges();
            db.Dispose();
        }

        public void SalvarComTotalCerteza()
        {
            db.SaveChanges();
            db.Dispose();
            db = new AplicativoContext();
        }

        public void AnalisarAdicionarClientes(IEnumerable<ClienteDI> clientes, DateTime atual)
        {
            foreach (var dest in clientes)
            {
                if (dest.Id != null && db.Clientes.Find(dest.Id) != null)
                {
                    dest.UltimaData = atual;
                    db.Update(dest);
                }
                else
                {
                    var busca = db.Clientes.FirstOrDefault(x => x.Documento == dest.Documento);
                    dest.UltimaData = atual;
                    if (busca != default(ClienteDI))
                    {
                        dest.Id = busca.Id;
                        db.Update(dest);
                    }
                    else
                    {
                        db.Add(dest);
                    }
                }
            }
        }

        public void AnalisarAdicionarMotoristas(IEnumerable<MotoristaDI> motoristas, DateTime atual)
        {
            foreach (var mot in motoristas)
            {
                if (mot.Id != null && db.Motoristas.Find(mot.Id) != null)
                {
                    mot.UltimaData = atual;
                    db.Update(mot);
                }
                else
                {
                    var busca = db.Motoristas.FirstOrDefault(x => x.Documento == mot.Documento
                        || (x.Nome == mot.Nome && x.XEnder == mot.XEnder));
                    mot.UltimaData = atual;
                    if (busca != default(MotoristaDI))
                    {
                        mot.Id = busca.Id;
                        db.Update(mot);
                    }
                    else
                    {
                        db.Add(mot);
                    }
                }
            }
        }

        public void AnalisarAdicionarProdutos(IEnumerable<ProdutoDI> produtos, DateTime atual)
        {
            foreach (var prod in produtos)
            {
                if (prod.Id != null && db.Produtos.Find(prod.Id) != null)
                {
                    prod.UltimaData = atual;
                    db.Update(prod);
                }
                else
                {
                    var busca = db.Produtos.FirstOrDefault(x => x.Descricao == prod.Descricao
                        || (x.CodigoProduto == prod.CodigoProduto && x.CFOP == prod.CFOP));
                    prod.UltimaData = atual;
                    if (busca != default(ProdutoDI))
                    {
                        prod.Id = busca.Id;
                        db.Update(prod);
                    }
                    else
                    {
                        db.Add(prod);
                    }
                }
            }
        }

        public void AdicionarNotasFiscais(IEnumerable<NFeDI> notas, DateTime atual)
        {
            foreach (var item in notas)
            {
                item.UltimaData = atual;
                if (db.NotasFiscais.Count(x => x.Id == item.Id) > 0)
                {
                    db.Update(item);
                }
                else
                {
                    db.Add(item);
                }
            }
        }

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

        public async Task AnalisarBanco(DateTime atual)
        {
            db.Database.Migrate();

            await db.Clientes.ForEachAsync(x => AnalisarItem(x));
            await db.Emitentes.ForEachAsync(x => AnalisarItem(x));
            await db.Motoristas.ForEachAsync(x => AnalisarItem(x));
            await db.Vendedores.ForEachAsync(x =>
            {
                if (string.IsNullOrEmpty(x.CPFStr))
                {
#pragma warning disable CS0612 // O tipo ou membro é obsoleto
                    x.CPFStr = x.CPF.ToString();
#pragma warning restore CS0612 // O tipo ou membro é obsoleto
                    db.Update(x);
                }
                AnalisarItem(x);
            });
            await db.Produtos.ForEachAsync(x => AnalisarItem(x));
            await db.Estoque.Include(x => x.Alteracoes).ForEachAsync(x =>
            {
                x.Alteracoes?.ForEach(alt =>
                {
                    if (alt.MomentoRegistro == default(DateTime))
                    {
                        alt.MomentoRegistro = atual;
                        db.Update(alt);
                    }
                });
                AnalisarItem(x);
            });
            await db.Vendas.ForEachAsync(x => AnalisarItem(x));
            await db.Imagens.ForEachAsync(x => AnalisarItem(x));

            void AnalisarItem(IUltimaData item)
            {
                if (item.UltimaData == DateTime.MinValue)
                {
                    item.UltimaData = atual;
                    db.Update(item);
                }
            }
        }

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

        public void SalvarComprador(Comprador item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public IEnumerable<VeiculoDI> ObterVeiculos() => db.Veiculos;

        public void SalvarMotorista(MotoristaDI item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void SalvarVeiculo(VeiculoDI item) => db.Veiculos.Add(item);

        public void SalvarVendedor(Vendedor item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void SalvarCliente(ClienteDI item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void SalvarProduto(ProdutoDI item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void SalvarNFe(NFeDI item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Status == (int)StatusNFe.Salva)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void SalvarEmitente(EmitenteDI item, DateTime atual)
        {
            item.UltimaData = atual;
            if (item.Id == Guid.Empty)
            {
                db.Add(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void AtualizarEstoque(Estoque item, DateTime atual)
        {
            item.UltimaData = atual;
            db.Update(item);
        }

        public IEnumerable<ProdutoDI> ObterProdutos()
        {
            return db.Produtos.Where(x => x.Ativo).OrderBy(x => x.Descricao);
        }

        public Estoque ObterEstoque(Guid id)
        {
            return db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == id);
        }

        public void AdicionarEstoque(Estoque item, DateTime atual)
        {
            item.UltimaData = atual;
            db.Add(item);
        }

        public void InativarProduto(ProdutoDI item, DateTime atual)
        {
            item.Ativo = false;
            item.UltimaData = atual;
            db.Update(item);
        }

        public void InativarCliente(ClienteDI item, DateTime atual)
        {
            item.Ativo = false;
            item.UltimaData = atual;
            db.Update(item);
        }

        public void InativarComprador(Comprador item, DateTime atual)
        {
            item.Ativo = false;
            item.UltimaData = atual;
            db.Update(item);
        }

        public void InativarMotorista(MotoristaDI item, DateTime atual)
        {
            item.Ativo = false;
            item.UltimaData = atual;
            db.Update(item);
        }

        public void InativarVendedor(Vendedor item, DateTime atual)
        {
            item.Ativo = false;
            item.UltimaData = atual;
            db.Update(item);
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
            return (from nota in db.NotasFiscais
                    where nota.CNPJEmitente == cnpj
                    where nota.SerieNota == serie
                    let notaHomologacao = nota.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                    where homologacao ? notaHomologacao : !notaHomologacao
                    select nota.NumeroNota).Max();
        }

        public (IEnumerable<NFeDI> emitidas, IEnumerable<NFeDI> outras, IEnumerable<NFeDI> canceladas) ObterNotas(string cnpj)
        {
            var notasFiscais = db.NotasFiscais.ToArray();
            var notasEmitidas = (from nota in notasFiscais
                             where nota.Status == (int)StatusNFe.Emitida
                             where nota.CNPJEmitente == cnpj
                             orderby nota.DataEmissao descending
                             select nota);
            var outrasNotas = (from nota in notasFiscais
                           where nota.Status != (int)StatusNFe.Emitida && nota.Status != (int)StatusNFe.Cancelada
                           where nota.CNPJEmitente == cnpj
                           orderby nota.DataEmissao descending
                           select nota);
            var notasCanceladas = (from nota in notasFiscais
                               where nota.Status == (int)StatusNFe.Cancelada
                               where nota.CNPJEmitente == cnpj
                               orderby nota.DataEmissao descending
                               select nota);
            return (notasEmitidas, outrasNotas, notasCanceladas);
        }

        public void ExcluirNFe(NFeDI item) => db.Remove(item);

        public void AtualizarNFe(NFeDI item, DateTime atual)
        {
            item.UltimaData = atual;
            db.Update(item);
        }

        public void AdicionarRC(RegistroCancelamento item) => db.Add(item);

        public void SalvarImagem(Guid id, DateTime atual, byte[] bytes)
        {
            var img = db.Imagens.Find(id);
            if (img != null)
            {
                img.UltimaData = atual;
                img.Bytes = bytes;
                db.Imagens.Update(img);
            }
            else
            {
                img = new Imagem()
                {
                    Id = id,
                    UltimaData = atual,
                    Bytes = bytes
                };
                db.Imagens.Add(img);
            }
        }

        public IEnumerable<(ClienteDI, Comprador[])> ObterClientesComCompradores()
        {
            var clients = db.Clientes.Where(x => x.Ativo).OrderBy(x => x.Nome).ToArray();
            for (int i = 0; i < clients.Length; i++)
            {
                var compradores = string.IsNullOrEmpty(clients[i].CNPJ) ? null : db.Compradores.Where(x => x.IdEmpresa == clients[i].Id).ToArray();
                yield return (clients[i], compradores);
            }
        }

        public void AdicionarRV(RegistroVenda ItemBanco, DateTime atual)
        {
            var produtosOrignal = ItemBanco.Produtos;
            ItemBanco.UltimaData = atual;
            ItemBanco.Produtos = null;
            db.Vendas.Add(ItemBanco);

            for (int i = 0; i < produtosOrignal.Count; i++)
            {
                var produto = produtosOrignal[i];
                var estoque = db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == produto.IdBase);
                if (estoque != null)
                {
                    estoque.UltimaData = atual;
                    estoque.Alteracoes.Add(new AlteracaoEstoque
                    {
                        Alteração = produto.Quantidade * -1,
                        MomentoRegistro = atual
                    });

                    db.Estoque.Update(estoque);
                }
            }
            SalvarComTotalCerteza();

            ItemBanco.Produtos = produtosOrignal;
            db.Vendas.Update(ItemBanco);
        }

        public void CancelarRV(RegistroVenda ItemBanco, CancelamentoRegistroVenda cancel, DateTime atual)
        {
            ItemBanco.UltimaData = atual;
            ItemBanco.Cancelado = true;
            db.Update(ItemBanco);

            for (int i = 0; i < ItemBanco.Produtos.Count; i++)
            {
                var produto = ItemBanco.Produtos[i];
                var estoque = db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == produto.IdBase);
                if (estoque != null)
                {
                    estoque.UltimaData = atual;
                    estoque.Alteracoes.Add(new AlteracaoEstoque
                    {
                        Alteração = produto.Quantidade,
                        MomentoRegistro = atual
                    });
                    db.Estoque.Update(estoque);
                }
            }

            db.CancelamentosRegistroVenda.Add(cancel);
        }

        public NFeDI ObterNFe(string id) => db.NotasFiscais.Find(id);
        public ClienteDI ObterCliente(Guid id) => db.Clientes.Find(id);
        public ClienteDI ObterClienteViaCNPJ(string cnpj) => db.Clientes.FirstOrDefault(x => x.CNPJ == cnpj);
        public MotoristaDI ObterMotorista(Guid id) => db.Motoristas.Find(id);
        public Vendedor ObterVendedor(Guid id) => db.Vendedores.Find(id);
        public Comprador ObterComprador(Guid id) => db.Compradores.Find(id);
        public ProdutoDI ObterProduto(Guid id) => db.Produtos.Find(id);
        public CancelamentoRegistroVenda ObterCRV(Guid id) => db.CancelamentosRegistroVenda.Find(id);

        public IEnumerable<(MotoristaDI, VeiculoDI, VeiculoDI[])> ObterMotoristasComVeiculos()
        {
            var mots = db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome).ToArray();
            for (int i = 0; i < mots.Length; i++)
            {
                var item1 = mots[i];
                VeiculoDI item2;
                VeiculoDI[] item3 = null;

                var secs = mots[i].VeiculosSecundarios;
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
                item2 = db.Veiculos.Find(mots[i].Veiculo);
                yield return (item1, item2, item3);
            }
        }

        public void ProcessarNFeLocal(string idOriginal, string novoId)
        {
            var notaAnterior = db.NotasFiscais.Find(idOriginal);
            if (notaAnterior != null)
            {
                db.NotasFiscais.Remove(notaAnterior);
            }

            var venda = db.Vendas.FirstOrDefault(x => x.NotaFiscalRelacionada == idOriginal);
            if (venda != null)
            {
                venda.NotaFiscalRelacionada = novoId;
                db.Vendas.Update(venda);
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

        public IEnumerable<int> ObterAnosNFe(string cnpjEmitente)
        {
            return (from dado in db.NotasFiscais
                    where dado.Status == (int)StatusNFe.Emitida
                    where dado.CNPJEmitente == cnpjEmitente
                    let ano = DateTime.Parse(dado.DataEmissao).Year
                    orderby ano ascending
                    select ano).Distinct();
        }

        public Dictionary<int, IEnumerable<(DateTime, string)>> ObterNFesPorAno(string cnpjEmitente)
        {
            return (from item in db.NotasFiscais
                    where item.Status == (int)StatusNFe.Emitida
                    where item.CNPJEmitente == cnpjEmitente
                    let data = DateTime.Parse(item.DataEmissao)
                    group new { Data = data, item.XML } by data.Year).ToDictionary(x => x.Key, x => x.Select(k => (k.Data, k.XML)));
        }
    }
}
