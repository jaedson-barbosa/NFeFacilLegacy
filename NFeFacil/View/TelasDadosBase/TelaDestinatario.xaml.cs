using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class TelaDestinatario : UserControl
    {
        public TelaDestinatario()
        {
            InitializeComponent();
        }

        private void rdbNacional_Checked(object sender, RoutedEventArgs e)
        {
            txtNomePaís.IsEnabled = false;
            txtCódigoPaís.IsEnabled = false;

            txtCEP.IsEnabled = true;
            cmbUF.IsEnabled = true;
            cmbMunicípio.IsEnabled = true;
        }

        private void rdbNacional_Unchecked(object sender, RoutedEventArgs e)
        {
            txtNomePaís.IsEnabled = true;
            txtCódigoPaís.IsEnabled = true;

            txtCEP.IsEnabled = false;
            cmbUF.IsEnabled = false;
            cmbMunicípio.IsEnabled = false;
        }

        private void chkIsento_Checked(object sender, RoutedEventArgs e)
        {
            txtIE.IsEnabled = false;
            txtIE.Text = "ISENTO";
        }

        private void chkIsento_Unchecked(object sender, RoutedEventArgs e)
        {
            txtIE.IsEnabled = true;
            txtIE.Text = string.Empty;
        }
    }
}
