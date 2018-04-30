using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using BaseGeral.ModeloXML.PartesDetalhes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Venda;
using Venda.Impostos;
using Venda.ViewProdutoVenda;

namespace Fiscal
{
    public abstract class ControleGenericoViewProdutoFiscal : IControleViewProdutoFiscal
    {
        public bool Concluido { get; private set; }
        public bool PodeConcluir { get; }
        public bool PodeDetalhar { get; }

        protected abstract List<DetalhesProdutos> Produtos { get; }
        protected abstract Total Total { set; }
        protected abstract void AbrirTelaDetalhamento(DadosAdicaoProduto dados);
        protected abstract void AbrirTelaEspecifica();

        public Dictionary<Guid, double> ProdutosAdicionados
        {
            get
            {
                using (var leitura = new BaseGeral.Repositorio.Leitura())
                    return (from x in Produtos
                            let id = leitura.ObterProduto(x.Produto.CodigoProduto).Id
                            let qt = x.Produto.QuantidadeComercializada
                            group qt by id)
                            .ToDictionary(x => x.Key, y => y.Sum());
            }
        }

        public ControleGenericoViewProdutoFiscal()
        {
            PodeConcluir = false;
            PodeDetalhar = true;
        }

        public ObservableCollection<ExibicaoProdutoListaGeral> ObterProdutosIniciais()
        {
            using (var leitura = new BaseGeral.Repositorio.Leitura())
                return (from prod in Produtos
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
                ValorUnitario = caixa.ProdutoSelecionado.Preco
            };
        }

        public void Adicionar(DetalhesProdutos produto)
        {
            var produtos = Produtos;
            var index = produtos.FindIndex(x => x.Número == produto.Número);
            if (index == -1) produtos.Add(produto);
            else produtos[index] = produto;
        }

        DadosAdicaoProduto ObterDadosAdicao(ProdutoDI prodBase, ProdutoSimplesVenda simples)
        {
            var produto = simples.ToProdutoOuServico();
            return new DadosAdicaoProduto(
                    prodBase,
                    new DetalhesProdutos()
                    {
                        Produto = produto,
                        Número = ObterNovoNumero()
                    })
            {
                IsNFCe = true
            };
        }

        async void AplicarTributacaoAutomatica(ProdutoDI prodBase, ProdutoSimplesVenda simples)
        {
            var dadosAdicao = ObterDadosAdicao(prodBase, simples);
            var tributador = new GerenciadorTributacao(dadosAdicao);
            var produtoTributado = await tributador.AplicarTributacaoAutomatica(false);
            Produtos.Add(produtoTributado);
        }

        public bool EdicaoLiberada { get; } = true;
        public void Editar(ExibicaoProdutoListaGeral produto)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                var prodDI = repo.ObterProduto(produto.Codigo);
                var completo = Produtos[FindProduto(produto)];
                var dados = new DadosAdicaoProduto(prodDI, completo)
                {
                    IsNFCe = true
                };
                AbrirTelaDetalhamento(dados);
            }
        }

        public void Remover(ExibicaoProdutoListaGeral produto)
        {
            var index = FindProduto(produto);
            Produtos.RemoveAt(index);
        }

        int FindProduto(ExibicaoProdutoListaGeral produto)
        {
            return Produtos.FindIndex(x => produto.Quantidade == x.Produto.QuantidadeComercializada
                && produto.ValorUnitario == x.Produto.ValorUnitario.ToString("C")
                && produto.Codigo == x.Produto.CodigoProduto);
        }

        public bool AnalisarDetalhamento(ProdutoAdicao produto)
        {
            var bProd = produto.Base;
            return (bProd.ImpostosPadrao == null || bProd.ImpostosPadrao.Length < 5)
                && (bProd.ImpostosSimples == null || bProd.ImpostosSimples.Length == 0);
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
            AbrirTelaDetalhamento(dados);
        }

        public void Avancar()
        {
            Produtos.Sort((x, y) => x.Número > y.Número ? 1 : x.Número < y.Número ? - 1 : 0);
            Total = new Total(Produtos);
            AbrirTelaEspecifica();
        }

        public void Concluir() => throw new NotImplementedException();

        public bool Validar()
        {
            if (Produtos.Any(x => x.Impostos.impostos.Count == 0))
            {
                Popup.Current.Escrever(TitulosComuns.Atenção, "Algum produto não tem nenhum imposto cadastrado, por favor, os insira para que possamos continuar com a criação da NFCe.");
                return false;
            }
            return true;
        }

        int ObterNovoNumero()
        {
            var produtos = Produtos;
            int numero = 0;
            if (produtos.Count == 0) numero = 1;
            else
            {
                var numeros = produtos.Select(x => x.Número);
                var maximo = produtos.Max(x => x.Número);
                for (int i = 1; i < maximo; i++)
                {
                    if (!numeros.Contains(i)) numero = i;
                }
                if (numero == 0) numero = maximo + 1;
            }
            return numero;
        }
    }
}
