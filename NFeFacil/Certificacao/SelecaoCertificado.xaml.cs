using NFeFacil.Primitivos;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Certificacao
{
    public sealed partial class SelecaoCertificado : ContentDialog
    {
        ObservableCollection<CertificadoExibicao> ListaCertificados { get; }
        public string CertificadoEscolhido { get; private set; }

        public SelecaoCertificado()
        {
            InitializeComponent();
            ListaCertificados = Task.Run(() => Certificados.ObterCertificadosAsync(ConfiguracoesCertificacao.Origem)).Result;
        }
    }
}
