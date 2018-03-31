using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.View;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    [DetalhePagina("Detalhamento de impostos", SimboloSymbol = Symbol.List)]
    public sealed partial class DetalhamentoGeral : Page
    {
        RoteiroAdicaoImpostos roteiro;

        public DetalhamentoGeral()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            roteiro = (RoteiroAdicaoImpostos)e.Parameter;
            Avancar();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (roteiro.Voltar())
            {
                frmImposto.GoBack();
                e.Cancel = true;
            }
        }

        private void Avancar(object sender, RoutedEventArgs e) => Avancar();

        void Avancar()
        {
            var atual = frmImposto.Content as Page;
            roteiro.ProcessarEntradaDados(atual);
            if (roteiro.Avancar())
            {
                if (roteiro.Current == null)
                {
                    frmImposto.Content = null;
                    Avancar();
                }
                else
                {
                    frmImposto.Navigate(roteiro.Current);
                }
            }
            else
            {
                Concluir();
            }
        }

        private void Voltar(object sender, RoutedEventArgs e)
        {
            if (roteiro.Voltar())
            {
                frmImposto.Navigate(roteiro.Current);
            }
            else
            {
                BasicMainPage.Current.Retornar();
            }
        }

        async void Concluir()
        {
            await Task.Delay(500);
            var produto = roteiro.Finalizar();

            var caixa = new DefinirTotalImpostos();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary && !string.IsNullOrEmpty(caixa.ValorTotalTributos))
            {
                produto.Impostos.vTotTrib = caixa.ValorTotalTributos;
            }
            else
            {
                produto.Impostos.vTotTrib = null;
            }

            //Remove tela de manipulação do produto e de escolha dos impostos
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);

            List<DetalhesProdutos> produtos;
            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter;
            if (parametro is NFe nfe)
            {
                var info = nfe.Informacoes;
                produtos = info.produtos;
                AddProduto();
                info.total = new Total(produtos);
            }
            else if (parametro is NFCe nfce)
            {
                var info = nfce.Informacoes;
                produtos = info.produtos;
                AddProduto();
                info.total = new Total(produtos);
            }
            else
            {
                throw new Exception();
            }

            BasicMainPage.Current.Retornar();

            void AddProduto()
            {
                if (produto.Número == 0)
                {
                    produto.Número = produtos.Count + 1;
                    produtos.Add(produto);
                }
                else
                {
                    produtos[produto.Número - 1] = produto;
                }
            }
        }

        private void ImpostoTrocado(object sender, NavigationEventArgs e)
        {
            var infoTipo = e.Content.GetType().GetTypeInfo();
            var detalhe = infoTipo.GetCustomAttribute<DetalhePagina>();
            txtTitulo.Text = detalhe.Titulo;
        }
    }
}
