using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static BaseGeral.Sincronizacao.ConfiguracoesSincronizacao;
using BaseGeral.Sincronizacao;
using BaseGeral;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    [DetalhePagina("\uE975", "Sincronização")]
    public sealed partial class SincronizacaoCliente : Page
    {
        public SincronizacaoCliente()
        {
            InitializeComponent();
        }

        bool estabelecendoConexao = false;
        void ConcluirEdicaoIP(UIElement sender, Windows.UI.Xaml.Input.LosingFocusEventArgs args)
        {
            if (estabelecendoConexao) return;
            var txtBox = (TextBox)sender;
            var newIP = txtBox.Text;
            if (newIP.Length == 0)
                txtBox.Text = CodigoServidor;
            else if (newIP.Length != 12)
                args.Cancel = true;
            else if (CodigoServidor != newIP)
                EstabelecerConexaoAsync(txtBox, newIP);
        }

        async void EstabelecerConexaoAsync(TextBox txtBox, string ip)
        {
            estabelecendoConexao = true;
            try
            {
                if (await new GerenciadorCliente().EstabelecerConexao())
                    CodigoServidor = ip;
                else
                    txtBox.Focus(FocusState.Keyboard);
            }
            catch (Exception ex)
            {
                ex.ManipularErro();
            }
            txtBox.Text = CodigoServidor;
            estabelecendoConexao = false;
        }

        async void SincronizarAgora(object sender, RoutedEventArgs e)
        {
            try
            {
                await new GerenciadorCliente().Sincronizar();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        async void SincronizarTudo(object sender, RoutedEventArgs e)
        {
            try
            {
                await new GerenciadorCliente().SincronizarTudo();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }
    }
}
