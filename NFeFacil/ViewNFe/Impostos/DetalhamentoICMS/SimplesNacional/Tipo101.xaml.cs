using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.SimplesNacional
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Tipo101 : Page
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public Tipo101()
        {
            InitializeComponent();
        }
    }
}
