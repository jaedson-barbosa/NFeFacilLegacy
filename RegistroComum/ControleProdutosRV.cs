using BaseGeral;
using BaseGeral.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Venda.ViewProdutoVenda;

namespace RegistroComum
{
    class ControleProdutosRV : IControleViewProduto
    {
        RegistroVenda ItemBanco;

        public bool Concluido { get; private set; }
        public bool PodeConcluir { get; }
        public bool PodeDetalhar { get; }
        public ObservableCollection<ProdutoGenericoVenda> Produtos { get; }

        public ControleProdutosRV()
        {
            Produtos = new ObservableCollection<ProdutoGenericoVenda>();
            PodeConcluir = false;
            PodeDetalhar = false;
        }

        public ControleProdutosRV(RegistroVenda registro)
        {
            using (var leitura = new BaseGeral.Repositorio.Leitura())
            {
                Produtos = (from prod in ItemBanco.Produtos
                            let comp = leitura.ObterProduto(prod.IdBase)
                            select new ProdutoGenericoVenda
                            {
                                IdBase = prod.IdBase,
                                Codigo = comp.CodigoProduto,
                                Desconto = prod.Desconto,
                                Descricao = comp.Descricao,
                                DespesasExtras = prod.DespesasExtras,
                                Frete = prod.Frete,
                                Quantidade = prod.Quantidade,
                                Seguro = prod.Seguro,
                                ValorUnitario = prod.ValorUnitario
                            }).GerarObs();
            }

            PodeConcluir = true;
            PodeDetalhar = false;
        }

        public void Avancar()
        {
            BasicMainPage.Current.Navegar<ManipulacaoRegistroVenda>();
        }

        public void Concluir()
        {
            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.SalvarRV(ItemBanco, DefinicoesTemporarias.DateTimeNow);
                Concluido = true;
                BasicMainPage.Current.Retornar();
            }
        }

        public void Detalhar()
        {
            throw new NotImplementedException();
        }
    }
}
