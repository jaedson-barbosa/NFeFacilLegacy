using BaseGeral.ModeloXML.PartesDetalhes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public interface IControleViewProduto
    {
        bool Concluido { get; }
        bool PodeConcluir { get; }
        bool PodeDetalhar { get; }
        Dictionary<Guid, double> ProdutosAdicionados { get; }

        ObservableCollection<ExibicaoProdutoListaGeral> ObterProdutosIniciais();
        bool AnalisarDetalhamento(ProdutoAdicao produto);
        bool AnalMudançaValorUnit(ProdutoAdicao produto);
        ExibicaoProdutoListaGeral Adicionar(AdicionarProduto caixa);
        bool EdicaoLiberada { get; }
        void Editar(ExibicaoProdutoListaGeral produto);
        void Remover(ExibicaoProdutoListaGeral produto);
        void Detalhar(AdicionarProduto caixa);

        void Voltar();
        void AtualizarControle(object atualizacao);

        bool Validar();
        void Avancar();
        void Concluir();
    }

    public interface IControleViewProdutoFiscal : IControleViewProduto
    {
        void Adicionar(DetalhesProdutos produto);
        void AplicarTributacaoAutomatica(DadosAdicaoProduto dadosAdicao);
    }
}
