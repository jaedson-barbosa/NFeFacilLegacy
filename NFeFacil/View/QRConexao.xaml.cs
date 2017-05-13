using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class QRConexao : Page, IEsconde, IValida
    {
        public QRConexao()
        {
            InitializeComponent();
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }

        public async Task<bool> Verificar()
        {
            if (GerenciadorServidor.Current.BrechaAberta)
            {
                new Popup().Escrever(TitulosComuns.Atenção, "Para voltar, primeiro aperte no botão que está presente no meio do carregamento circular para que conexões de novos dispositivos não sejam mais aceitas.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
