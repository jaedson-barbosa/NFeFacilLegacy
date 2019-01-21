using BaseGeral;
using BaseGeral.Log;
using BaseGeral.Sincronizacao;
using BaseGeral.View;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static BaseGeral.Sincronizacao.ConfiguracoesSincronizacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    [DetalhePagina("\uE977", "Sincronização")]
    public sealed partial class SincronizacaoServidor : Page
    {
        readonly string IP;

        public SincronizacaoServidor()
        {
            InitializeComponent();

            try
            {
                var hosts = NetworkInformation.GetHostNames();
                IP = hosts.Count > 0
                    ? hosts.Last(x => x.IPInformation != null && x.Type == HostNameType.Ipv4).ToString().IPToCodigo()
                    : "Erro";
                GerenciadorServidor.AceitarNovasConexoes = true;
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            GerenciadorServidor.AceitarNovasConexoes = false;
        }

        async void IniciarServidor(object sender, RoutedEventArgs e)
        {
            try
            {
                await GerenciadorServidor.IniciarServer();
            }
            catch (COMException)
            {
                Popup.Current.Escrever(TitulosComuns.Erro, "O servidor já está ativo.");
            }
            catch (Exception ex)
            {
                ex.ManipularErro();
            }
            ((Button)sender).IsEnabled = GerenciadorServidor.Inativo;
        }
    }
}
