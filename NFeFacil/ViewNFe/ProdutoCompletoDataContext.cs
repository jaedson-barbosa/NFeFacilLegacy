using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewNFe
{
    public sealed class ProdutoCompletoDataContext
    {
        public DetalhesProdutos ProdutoCompleto { get; }

        public ObservableCollection<DeclaracaoImportacao> ListaDI { get; } = new ObservableCollection<DeclaracaoImportacao>();
        public ObservableCollection<GrupoExportacao> ListaGE { get; } = new ObservableCollection<GrupoExportacao>();

        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (ProdutoCompleto.ImpostoDevol == null)
                {
                    ProdutoCompleto.ImpostoDevol = new ImpostoDevol();
                }
                return ProdutoCompleto.ImpostoDevol;
            }
        }

        public ProdutoCompletoDataContext(DetalhesProdutos produtoCompleto)
        {
            ProdutoCompleto = produtoCompleto;

            AdicionarDeclaracaoImportacaoCommand = new Comando(AdicionarDeclaracaoImportacao);
            AdicionarDeclaracaoExportacaoCommand = new Comando(AdicionarDeclaracaoExportacao);
            RemoverDeclaracaoImportacaoCommand = new Comando<DeclaracaoImportacao>(RemoverDeclaracaoImportacao);
            RemoverDeclaracaoExportacaoCommand = new Comando<GrupoExportacao>(RemoverDeclaracaoExportacao);
        }

        public ICommand AdicionarDeclaracaoImportacaoCommand { get; }
        public ICommand AdicionarDeclaracaoExportacaoCommand { get; }
        public ICommand RemoverDeclaracaoImportacaoCommand { get; }
        public ICommand RemoverDeclaracaoExportacaoCommand { get; }

        private async void AdicionarDeclaracaoImportacao()
        {
            var caixa = new CaixasDialogoProduto.AdicionarDeclaracaoImportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ListaDI.Add(caixa.Declaracao);
            }
        }

        private async void AdicionarDeclaracaoExportacao()
        {
            var caixa = new CaixasDialogoProduto.EscolherTipoDeclaracaoExportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var tipo = caixa.TipoEscolhido;
                if (tipo == CaixasDialogoProduto.TiposDeclaracaoExportacao.Direta)
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoDireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
                else
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoIndireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
            }
        }

        private void RemoverDeclaracaoImportacao(DeclaracaoImportacao declaracao)
        {
            ListaDI.Remove(declaracao);
        }

        private void RemoverDeclaracaoExportacao(GrupoExportacao declaracao)
        {
            ListaGE.Remove(declaracao);
        }

        void Concluir()
        {
            ProdutoCompleto.Produto.DI = new List<DeclaracaoImportacao>(ListaDI);
            ProdutoCompleto.Produto.GrupoExportação = new List<GrupoExportacao>(ListaGE);
        }
    }
}
