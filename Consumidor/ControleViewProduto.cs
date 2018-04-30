using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Venda;
using Venda.Impostos;
using Venda.ViewProdutoVenda;

namespace Consumidor
{
    public sealed class ControleViewProduto : IControleViewProduto
    {
        public bool Concluido { get; private set; }
        public bool PodeConcluir { get; }
        public bool PodeDetalhar { get; }

        NFCe Venda { get; }
        public Guid[] ProdutosAdicionados
        {
            get
            {
                using (var leitura = new BaseGeral.Repositorio.Leitura())
                    return (from x in Venda.Informacoes.produtos
                            select leitura.ObterProduto(x.Produto.CodigoProduto).Id).ToArray();
            }
        }

        public ControleViewProduto(NFCe venda)
        {
            Venda = venda;
            PodeConcluir = false;
            PodeDetalhar = true;
        }

        public ObservableCollection<ExibicaoProdutoListaGeral> ObterProdutosIniciais()
        {
            using (var leitura = new BaseGeral.Repositorio.Leitura())
                return (from prod in Venda.Informacoes.produtos
                        let comp = leitura.ObterProduto(prod.Produto.CodigoProduto)
                        let intP = prod.Produto
                        select new ExibicaoProdutoListaGeral
                        {
                            Codigo = comp.CodigoProduto,
                            Descricao = comp.Descricao,
                            Quantidade = intP.QuantidadeComercializada,
                            ValorUnitario = intP.ValorUnitario.ToString("C"),
                            TotalLiquido = intP.ValorTotal.ToString("C")
                        }).GerarObs();
        }

        public ExibicaoProdutoListaGeral Adicionar(AdicionarProduto caixa)
        {
            var simples = new ProdutoSimplesVenda
            {
                IdBase = caixa.ProdutoSelecionado.Base.Id,
                ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble,
                Quantidade = caixa.Quantidade,
                Frete = 0,
                Seguro = caixa.Seguro,
                DespesasExtras = caixa.DespesasExtras
            };
            simples.CalcularTotalLíquido();
            AplicarTributacaoAutomatica(caixa.ProdutoSelecionado.Base, simples);
            return new ExibicaoProdutoListaGeral
            {
                Codigo = caixa.ProdutoSelecionado.Codigo,
                Descricao = caixa.ProdutoSelecionado.Nome,
                Quantidade = caixa.Quantidade,
                TotalLiquido = simples.TotalLíquido.ToString("C"),
                ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble.ToString("C")
            };
        }

        DadosAdicaoProduto ObterDadosAdicao(ProdutoDI prodBase, ProdutoSimplesVenda simples)
        {
            var produto = simples.ToProdutoOuServico();
            return new DadosAdicaoProduto(
                    prodBase,
                    new BaseGeral.ModeloXML.PartesDetalhes.DetalhesProdutos()
                    {
                        Produto = produto,
                        Número = Venda.Informacoes.produtos.Max(x => x.Número) + 1
                    })
            {
                IsNFCe = true
            };
        }

        async void AplicarTributacaoAutomatica(ProdutoDI prodBase, ProdutoSimplesVenda simples)
        {
            var dadosAdicao = ObterDadosAdicao(prodBase, simples);
            var tributador = new GerenciadorTributacao(dadosAdicao);
            var produtoTributado = await tributador.AplicarTributacaoAutomatica();
            Venda.Informacoes.produtos.Add(produtoTributado);
        }

        public bool EdicaoLiberada { get; } = true;
        public void Editar(ExibicaoProdutoListaGeral produto)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                var prodDI = repo.ObterProduto(produto.Codigo);
                var completo = Venda.Informacoes.produtos[FindProduto(produto)];
                var dados = new DadosAdicaoProduto(prodDI, completo)
                {
                    IsNFCe = true
                };
                BasicMainPage.Current.Navegar<ProdutoNFCe>(dados);
            }
        }

        public void Remover(ExibicaoProdutoListaGeral produto)
        {
            var valorUnit = double.Parse(produto.ValorUnitario);
            var index = FindProduto(produto);
            Venda.Informacoes.produtos.RemoveAt(index);
        }

        int FindProduto(ExibicaoProdutoListaGeral produto)
        {
            var valorUnit = double.Parse(produto.ValorUnitario);
            return Venda.Informacoes.produtos.FindIndex(x => produto.Quantidade == x.Produto.QuantidadeComercializada
                && valorUnit == x.Produto.ValorUnitario && produto.Codigo == x.Produto.CodigoProduto);
        }

        public bool AnalisarDetalhamento(ExibicaoProdutoAdicao produto)
        {
            return produto.Base.ImpostosPadrao.Length < 5
                && produto.Base.ImpostosSimples.Length == 0;            
        }

        public void Detalhar(AdicionarProduto caixa)
        {
            var simples = new ProdutoSimplesVenda
            {
                IdBase = caixa.ProdutoSelecionado.Base.Id,
                ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble,
                Quantidade = caixa.Quantidade,
                Frete = 0,
                Seguro = caixa.Seguro,
                DespesasExtras = caixa.DespesasExtras
            };
            simples.CalcularTotalLíquido();
            var dados = ObterDadosAdicao(caixa.ProdutoSelecionado.Base, simples);
            BasicMainPage.Current.Navegar<ProdutoNFCe>(dados);
        }

        public void Avancar()
        {
            Venda.Informacoes.total = new Total(Venda.Informacoes.produtos);
            BasicMainPage.Current.Navegar<ManipulacaoNFCe>(Venda);
        }

        public void Concluir() => throw new NotImplementedException();

        public bool Validar()
        {
            if (Venda.Informacoes.produtos.Any(x => x.Impostos.impostos.Count == 0))
            {
                Popup.Current.Escrever(TitulosComuns.Atenção, "Algum produto não tem nenhum imposto cadastrado, por favor, os insira para que possamos continuar com a criação da NFCe.");
                return false;
            }
            return true;
        }
    }
}
