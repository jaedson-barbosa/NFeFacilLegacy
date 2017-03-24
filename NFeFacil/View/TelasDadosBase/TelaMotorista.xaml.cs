using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class TelaMotorista : UserControl
    {
        public TelaMotorista()
        {
            InitializeComponent();
        }

        private void chkIsento_Checked(object sender, RoutedEventArgs e)
        {
            txtIE.IsEnabled = false;
            txtIE.Text = "ISENTO";
        }

        private void chkIsento_Unchecked(object sender, RoutedEventArgs e)
        {
            txtIE.IsEnabled = true;
            txtIE.Text = null;
        }
    }
}
