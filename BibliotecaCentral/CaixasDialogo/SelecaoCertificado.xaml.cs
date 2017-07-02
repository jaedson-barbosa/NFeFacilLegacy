using BibliotecaCentral.Certificacao;
using Comum.Primitivos;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace BibliotecaCentral.CaixasDialogo
{
    public sealed partial class SelecaoCertificado : ContentDialog
    {
        ObservableCollection<CertificadoExibicao> ListaCertificados { get; }
        public string CertificadoEscolhido { get; private set; }

        public SelecaoCertificado()
        {
            this.InitializeComponent();
            var origem = ConfiguracoesCertificacao.Origem;
            ListaCertificados = Task.Run(() => Certificados.ObterCertificadosAsync(origem)).Result;
        }
    }
}
