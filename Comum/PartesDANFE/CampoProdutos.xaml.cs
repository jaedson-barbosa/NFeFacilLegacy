using NFeFacil.Fiscal.ViewNFe.PacotesDANFE;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static BaseGeral.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Fiscal.ViewNFe.PartesDANFE
{
    public sealed partial class CampoProdutos : UserControl
    {
        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();
        DimensoesCampoProdutos DimensoesLocal { get; } = new DimensoesCampoProdutos();

        public ObservableCollection<DadosProduto> Contexto { get; } = new ObservableCollection<DadosProduto>();

        public CampoProdutos()
        {
            InitializeComponent();
        }
    }

    public sealed class DimensoesCampoProdutos
    {
        public GridLength Coluna0 => CMToLength(1.5);
        public GridLength Coluna1 => CMToLength(4.75);
        public GridLength Coluna2 => CMToLength(1.25);
        public GridLength ColunaGeral3 => CMToLength(1.5);
        public GridLength ColunaGeral4 => CMToLength(2);
        public GridLength ColunaGeral5 => CMToLength(6.5);
        public GridLength ColunaGeral6 => CMToLength(1.5);

        public GridLength LinhaPadrao => CMToLength(0.55);
        public double AlturaPadrao => LinhaPadrao.Value;
    }

    public sealed class EsconderCasoNulo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                return string.IsNullOrEmpty(str) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return value == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
