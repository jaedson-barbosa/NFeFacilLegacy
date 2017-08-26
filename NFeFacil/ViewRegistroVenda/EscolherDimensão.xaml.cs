using NFeFacil.Log;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class EscolherDimensão : ContentDialog
    {
        bool formularioContinuo;
        public bool FormularioContinuo
        {
            get => formularioContinuo;
            set
            {
                formularioContinuo = value;
                txtAltura.IsEnabled = !value;
            }
        }
        public double Altura { get; set; }
        public double Largura { get; set; }

        public EscolherDimensão()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var log = Popup.Current;
            if (Altura < 14 && !FormularioContinuo)
            {
                log.Escrever(TitulosComuns.Atenção, "A altura está muito pequena e é provável que os elementos não caibam, por favor, escolha uma altura maior.");
                args.Cancel = true;
            }
            else if (Largura < 18)
            {
                log.Escrever(TitulosComuns.Atenção, "A largura está muito pequena e é provável que os elementos não caibam, por favor, escolha uma largura maior.");
                args.Cancel = true;
            }
        }
    }
}
