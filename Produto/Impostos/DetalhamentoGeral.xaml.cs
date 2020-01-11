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
            var telas = roteiro.Telas;
            for (int i = 0; i < telas.Length; i++)
                if (telas[i] != null)
                {
                    var infoTipo = telas[i].GetType().GetTypeInfo();
                    var detalhe = infoTipo.GetCustomAttribute<DetalhePagina>();
                    stkImpostos.Children.Add(new TextBlock
                    {
                        Margin = new Thickness(0, 16, 0, 0),
                        Text = detalhe.Titulo,
                        Style = (Style)Resources["TitleTextBlockStyle"]
                    });
                    stkImpostos.Children.Add(telas[i]);
                }
        }

        async void Concluir(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            var produto = roteiro.Finalizar();

            var caixa = new DefinirTotalImpostos();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary && caixa.ValorTotalTributos != 0)
            {
                produto.Impostos.vTotTrib = ExtensoesPrincipal.ToStr(caixa.ValorTotalTributos);
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
    }
}
