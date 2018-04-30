using System;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public interface IControleViewProduto
    {
        bool Concluido { get; }
        bool PodeConcluir { get; }
        bool PodeDetalhar { get; }
        Guid[] ProdutosAdicionados { get; }

        ObservableCollection<ExibicaoProdutoListaGeral> ObterProdutosIniciais();
        bool AnalisarDetalhamento(ExibicaoProdutoAdicao produto);
        ExibicaoProdutoListaGeral Adicionar(AdicionarProduto caixa);
        bool EdicaoLiberada { get; }
        void Editar(ExibicaoProdutoListaGeral produto);
        void Remover(ExibicaoProdutoListaGeral produto);
        void Detalhar(AdicionarProduto caixa);

        bool Validar();
        void Avancar();
        void Concluir();
    }
}
