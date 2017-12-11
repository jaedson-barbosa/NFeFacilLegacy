using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class DadosNFe : UserControl
    {
        public Pacotes.DadosNFe Contexto { get; set; }

        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();

        GridLength Coluna0 => CentimeterToLength(8);
        GridLength Coluna1 => CentimeterToLength(3);
        GridLength Coluna2 => CentimeterToLength(8);

        GridLength Linha0 => CentimeterToLength(3.4);

        public DadosNFe()
        {
            InitializeComponent();
        }

        public void DefinirPagina(int paginaAtual, int total)
        {
            txtPagAtual.Text = paginaAtual.ToString();
            txtPagTotal.Text = total.ToString();
        }
    }
}
