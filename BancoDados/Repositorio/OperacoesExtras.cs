using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFeFacil.Repositorio
{
    public sealed class OperacoesExtras : IDisposable
    {
        AplicativoContext db = new AplicativoContext();

        public void Dispose()
        {
            db.SaveChanges();
            db.Dispose();
        }

        public async Task AnalisarBanco(DateTime atual)
        {
            db.Database.Migrate();

            //db.SaveChanges();
            //db.Dispose();
            //db = new AplicativoContext();

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
            await db.Veiculos.ForEachAsync(x => AnalisarItem(x));

            void AnalisarItem(IUltimaData item)
            {
                if (item.UltimaData == DateTime.MinValue)
                {
                    item.UltimaData = atual;
                    db.Update(item);
                }
            }
        }

        public void ExcluirNFe(NFeDI item) => db.Remove(item);

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
    }
}
