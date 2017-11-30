using NFeFacil.Log;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class EscolherDimensão : ContentDialog
    {
        int predefinicao = 1;
        public int Predefinicao
        {
            get => predefinicao;
            set
            {
                predefinicao = value;
                switch (value)
                {
                    case 0:
                        txtAltura.IsEnabled = false;
                        txtLargura.IsEnabled = true;
                        break;
                    case 1:
                        txtAltura.IsEnabled = true;
                        txtLargura.IsEnabled = true;
                        break;
                    case 2:
                        Largura = 21;
                        Altura = 29.7;
                        txtAltura.IsEnabled = false;
                        txtLargura.IsEnabled = false;
                        break;
                    case 3:
                        Largura = 29.7;
                        Altura = 21;
                        txtAltura.IsEnabled = false;
                        txtLargura.IsEnabled = false;
                        break;
                }
            }
        }
        public double Altura { get; private set; }
        public double Largura { get; private set; }

        public EscolherDimensão()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var log = Popup.Current;
            if (Altura < 14 && Predefinicao != 0)
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
