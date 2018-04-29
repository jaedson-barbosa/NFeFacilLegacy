using BaseGeral;
using BaseGeral.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Venda.ViewProdutoVenda;

namespace RegistroComum
{
    public sealed class ControleViewProduto : IControleViewProduto
    {
        public bool Concluido { get; private set; }
        public bool PodeConcluir { get; }
        public bool PodeDetalhar { get; }

        RegistroVenda Venda { get; }
        public Guid[] ProdutosAdicionados => Venda.Produtos.Select(x => x.IdBase).ToArray();

        public ControleViewProduto()
        {
            Venda = new RegistroVenda();
            PodeConcluir = false;
            PodeDetalhar = true;
        }

        public ControleViewProduto(RegistroVenda venda)
        {
            Venda = venda;
            PodeConcluir = true;
            PodeDetalhar = false;
        }

        public ObservableCollection<ExibicaoProdutoListaGeral> ObterProdutosIniciais()
        {
            using (var leitura = new BaseGeral.Repositorio.Leitura())
                return (from prod in Venda.Produtos
                        let comp = leitura.ObterProduto(prod.IdBase)
                        select new ExibicaoProdutoListaGeral
                        {
                            Codigo = comp.CodigoProduto,
                            Descricao = comp.Descricao,
                            Quantidade = prod.Quantidade,
                            ValorUnitario = prod.ValorUnitario.ToString("C"),
                            TotalLiquido = prod.TotalLíquido.ToString("C")
                        }).GerarObs();
        }

        public ExibicaoProdutoListaGeral Adicionar(AdicionarProduto caixa)
        {
            var novoProdBanco = new ProdutoSimplesVenda
            {
                IdBase = caixa.ProdutoSelecionado.Base.Id,
                ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble,
                Quantidade = caixa.Quantidade,
                Frete = 0,
                Seguro = caixa.Seguro,
                DespesasExtras = caixa.DespesasExtras
            };
            novoProdBanco.CalcularTotalLíquido();
            Venda.Produtos.Add(novoProdBanco);
            return new ExibicaoProdutoListaGeral
            {
                Codigo = caixa.ProdutoSelecionado.Codigo,
                Descricao = caixa.ProdutoSelecionado.Nome,
                Quantidade = caixa.Quantidade,
                TotalLiquido = novoProdBanco.TotalLíquido.ToString("C"),
                ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble.ToString("C")
            };
        }

        public void Remover(ExibicaoProdutoListaGeral produto)
        {
            var valorUnit = double.Parse(produto.ValorUnitario);
            var totalLiqu = double.Parse(produto.TotalLiquido);
            var index = Venda.Produtos.FindIndex(x => produto.Quantidade == x.Quantidade
                && valorUnit == x.ValorUnitario && totalLiqu == x.TotalLíquido);
            Venda.Produtos.RemoveAt(index);
        }

        public bool AnalisarDetalhamento(ExibicaoProdutoAdicao produto) => throw new NotImplementedException();
        public void Detalhar() => throw new NotImplementedException();

        public void Avancar() => BasicMainPage.Current.Navegar<ManipulacaoRegistroVenda>();

        public void Concluir()
        {
            using (var repo = new BaseGeral.Repositorio.Escrita())
                repo.SalvarRV(Venda, DefinicoesTemporarias.DateTimeNow);
            Concluido = true;
            BasicMainPage.Current.Retornar();
        }

        public bool Validar() => true;

        sealed class ProdutoVenda : ProdutoGenericoVenda { }
    }
}
