using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos
{
    [View.DetalhePagina("Detalhamento de impostos", SimboloSymbol = Symbol.List)]
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
            if (roteiro.Validar(atual))
            {
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
        }

        private void Voltar(object sender, RoutedEventArgs e)
        {
            if (roteiro.Voltar())
            {
                frmImposto.Navigate(roteiro.Current);
            }
            else
            {
                MainPage.Current.Retornar();
            }
        }

        async void Concluir()
        {
            await Task.Delay(500);
            var produto = roteiro.Finalizar();

            var caixa = new DefinirTotalImpostos();
            await caixa.ShowAsync();
            if (!string.IsNullOrEmpty(caixa.ValorTotalTributos))
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

            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter as NFe;
            var info = parametro.Informacoes;

            if (produto.Número == 0)
            {
                produto.Número = info.produtos.Count + 1;
                info.produtos.Add(produto);
            }
            else
            {
                info.produtos[produto.Número - 1] = produto;
            }
            info.total = new Total(info.produtos);

            MainPage.Current.Retornar();
        }

        private void ImpostoTrocado(object sender, NavigationEventArgs e)
        {
            var infoTipo = e.Content.GetType().GetTypeInfo();
            var detalhe = infoTipo.GetCustomAttribute<View.DetalhePagina>();
            txtTitulo.Text = detalhe.Titulo;
        }
    }
}
