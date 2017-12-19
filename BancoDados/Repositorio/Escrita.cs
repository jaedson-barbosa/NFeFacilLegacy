using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Repositorio
{
    public sealed class Escrita : IDisposable
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

        public void SalvarDadoBase(IUltimaData item, DateTime atual)
        {
            if (item is Estoque && item.UltimaData == DateTime.MinValue)
            {
                item.UltimaData = atual;
                db.Add(item);
            }
            else
            {
                item.UltimaData = atual;
                if (item is IGuidId guid && guid.Id == Guid.Empty) db.Add(item);
                else if (item is IStatusAtual sts && sts.Status == sts.StatusAdd) db.Add(item);
                else db.Update(item);
            }
        }

        public void InativarDadoBase(IStatusAtivacao item, DateTime atual)
        {
            item.Ativo = false;
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
    }
}
