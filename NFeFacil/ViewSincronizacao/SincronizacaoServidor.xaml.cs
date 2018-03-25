using BaseGeral;
using BaseGeral.Log;
using BaseGeral.Sincronizacao;
using BaseGeral.View;
using System;
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static BaseGeral.Sincronizacao.ConfiguracoesSincronizacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    [DetalhePagina("\uE977", "Sincronização")]
    public sealed partial class SincronizacaoServidor : Page
    {
        public SincronizacaoServidor()
        {
            InitializeComponent();
        }

        public bool IniciarAutomaticamente
        {
            get => InícioAutomático;
            set => InícioAutomático = value;
        }

        async void IniciarServidor(object sender, RoutedEventArgs e)
        {
            try
            {
                await GerenciadorServidor.Current.IniciarServer();
            }
            catch (COMException)
            {
                Popup.Current.Escrever(TitulosComuns.Erro, "O servidor já está ativo.");
            }
            catch (Exception ex)
            {
                ex.ManipularErro();
            }
        }

        void ExibirQR(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<QRConexao>();
        }
    }
}
