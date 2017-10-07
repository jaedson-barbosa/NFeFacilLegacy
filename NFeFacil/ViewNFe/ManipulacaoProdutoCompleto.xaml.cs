﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using NFeFacil.View.Controles;
using System.Threading.Tasks;
using Windows.UI.Popups;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer, IValida
    {
        public ManipulacaoProdutoCompleto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var produto = (DetalhesProdutos)e.Parameter;
            if (produto.Impostos.impostos.Count > 0)
            {
                MainPage.Current.SeAtualizar(Symbol.Edit, "Produto");
                DataContext = new ProdutoCompletoDataContext(produto);
            }
            else
            {
                MainPage.Current.SeAtualizar(Symbol.Add, "Produto");
                DataContext = new ProdutoCompletoDataContext(produto);
            }
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Tag, "Dados"),
            new ItemHambuguer("\uE825", "Imposto devolvido"),
            new ItemHambuguer(Symbol.Comment, "Info adicional"),
            new ItemHambuguer(Symbol.World, "Importação"),
            new ItemHambuguer(Symbol.World, "Exportação"),
            new ItemHambuguer(Symbol.Target, "Produto específico")
        };

        public void AtualizarMain(int index) => main.SelectedIndex = index;

        private void Concluir_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as ProdutoCompletoDataContext;
            var porcentDevolv = data.ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                data.ProdutoCompleto.ImpostoDevol = null;
            }
            MainPage.Current.Navegar<ImpostosProduto>(data.ProdutoCompleto);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        async Task<bool> IValida.Verificar()
        {
            var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, escolha Cancelar.\r\n" +
                "Mas lembre-se que, caso o produto já tenha sido salvo, as alterações não terão efeito, e caso contrário, o produto não será adicionado.", "Atenção");
            mensagem.Commands.Add(new UICommand("Sair"));
            mensagem.Commands.Add(new UICommand("Cancelar"));
            var resultado = await mensagem.ShowAsync();
            return resultado.Label == "Sair";
        }

        private void TelaMudou(object sender, SelectionChangedEventArgs e)
        {
            var index = ((FlipView)sender).SelectedIndex;
            MainPage.Current.AlterarSelectedIndexHamburguer(index);
        }
    }
}