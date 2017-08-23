using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoProdutos : UserControl
    {
        public CampoProdutos()
        {
            InitializeComponent();
        }
    }

    public sealed class DimensoesCampoProdutos
    {
        public GridLength Coluna0 => DimensoesPadrao.CentimeterToLength(1.5);
        public GridLength Coluna1 => DimensoesPadrao.CentimeterToLength(4.75);
        public GridLength Coluna2 => DimensoesPadrao.CentimeterToLength(1.25);
        public GridLength ColunaGeral3 => DimensoesPadrao.CentimeterToLength(1.5);
        public GridLength ColunaGeral4 => DimensoesPadrao.CentimeterToLength(2);
        public GridLength ColunaGeral5 => DimensoesPadrao.CentimeterToLength(6.5);
        public GridLength ColunaGeral6 => DimensoesPadrao.CentimeterToLength(1.5);

        public GridLength LinhaPadrao => DimensoesPadrao.CentimeterToLength(0.55);
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
