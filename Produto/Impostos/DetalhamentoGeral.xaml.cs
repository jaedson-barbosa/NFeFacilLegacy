using BaseGeral;
using BaseGeral.View;
using System;
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

        void Avancar(object sender, RoutedEventArgs e) => Avancar();

        void Avancar()
        {
            roteiro.Processar(frmImposto.Content as Page);
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

            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter;
            if (parametro is ViewProdutoVenda.IControleViewProdutoFiscal controle)
            {
                controle.Adicionar(produto);
            }
            else
            {
                throw new Exception();
            }

            BasicMainPage.Current.Retornar();
        }

        private void ImpostoTrocado(object sender, NavigationEventArgs e)
        {
            var infoTipo = e.Content.GetType().GetTypeInfo();
            var detalhe = infoTipo.GetCustomAttribute<DetalhePagina>();
            txtTitulo.Text = detalhe.Titulo;
        }
    }
}
