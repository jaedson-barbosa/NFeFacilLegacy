using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.SimplesNacional
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Tipo201 : Page
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public string vBCST { get; private set; }
        public string pICMSST { get; private set; }
        public string vICMSST { get; private set; }

        public Tipo201()
        {
            this.InitializeComponent();
        }
    }
}
