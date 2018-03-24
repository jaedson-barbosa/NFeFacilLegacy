using static BaseGeral.ExtensoesPrincipal;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda.DARV
{
    public sealed class ProdutosDARV
    {
        public ObservableCollection<ExibicaoProduto> Produtos { get; set; }

        public static GridLength Largura2 { get; } = CMToLength(2);
        public static GridLength Largura3 { get; } = CMToLength(3);
    }
}
