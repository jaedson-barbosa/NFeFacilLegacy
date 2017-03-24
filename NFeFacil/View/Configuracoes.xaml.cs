using NFeFacil.NavegacaoUI;
using NFeFacil.View.Controles;
using NFeFacil.ViewModel.Configuracoes;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Configuracoes : Page
    {
        private readonly ViewModel.Configuracoes.Sincronizacao DataContextSincronização;

        public CarregamentoCircular Carregamento
        {
            get { return carTempo; }
        }

        public Grid GridQR
        {
            get { return grdInfoImgQR; }
        }

        public Configuracoes()
        {
            InitializeComponent();
            pvtImportação.DataContext = new ViewModel.Configuracoes.Importacao();
            pvtCertificação.DataContext = new Certificacao();
            DataContext = DataContextSincronização = new ViewModel.Configuracoes.Sincronizacao(this);
            Propriedades.Intercambio.SeAtualizar(Telas.Configurações, Symbol.Setting, nameof(Configuracoes));
        }

        public async Task MostrarQRTemporario()
        {
            MostrarQR.Begin();
            await Task.Delay(1000);
        }

        public void OcultarQRTemporario() => OcultarQR.Begin();

        public async Task Esconder()
        {
            OcultarGrid.Begin();
            await Task.Delay(250);
        }

        private void tglSincronizarDadosBase_Toggled(object sender, RoutedEventArgs e)
        {
            /*if (tglSincronizarDadosBase.IsOn) MostrarStkDadosBase.Begin();
            else EsconderStkDadosBase.Begin();*/
        }

        private async void grdQRTemporario_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (grdPrincipal.ActualWidth > 500)
            {
                grdImgQR.Style = (Style)Resources["ImagemHorizontal"];
                grdInfoImgQR.Style = (Style)Resources["InfoImagemHorizontal"];
            }
            else
            {
                grdImgQR.Style = (Style)Resources["ImagemVertical"];
                grdInfoImgQR.Style = (Style)Resources["InfoImagemVertical"];
            }
            await Task.Delay(100);
            DataContextSincronização.OnProperyChanged("QRGerado");
        }
    }
}
