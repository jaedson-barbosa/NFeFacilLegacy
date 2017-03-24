using NFeFacil.Log;
using System;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace NFeFacil.NavegacaoUI
{
    internal static class BarraStatus
    {
        internal static void Processar()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
            {
                var barra = new BarraStatusCelular();
                barra.FecharBarra();
            }
            else if (familia.Contains("Desktop"))
            {
                var barra = new BarraStatusPC();
                var cor = new UISettings().GetColorValue(UIColorType.Accent);
                barra.DefinirCor(cor, cor);
                BarraStatusPC.MostrarBotãoVoltar();
            }
            else
            {
                ILog log = new Popup();
                log.Escrever(TitulosComuns.ErroSimples, "Tipo não reconhecido de dispositivo, não é possível mudar a barra de título.");
            }
        }

        private class BarraStatusCelular
        {
            private readonly StatusBar Barra = StatusBar.GetForCurrentView();

            public async void MostrarBarra() => await Barra.ShowAsync();
            public async void FecharBarra() => await Barra.HideAsync();

            public void DefinirCor(Color corDeFundo) => Barra.BackgroundColor = corDeFundo;
            public void DefinirCor(Color corDeFundo, Color corDeEscrita)
            {
                DefinirCor(corDeFundo);
                Barra.ForegroundColor = corDeEscrita;
            }

            public async void DefinirProgresso(string oqAcontece)
            {
                Barra.ProgressIndicator.Text = oqAcontece;
                Barra.ProgressIndicator.ProgressValue = 0;
                await Barra.ProgressIndicator.ShowAsync();
            }
            public void AttProgresso(double porcentagem) => Barra.ProgressIndicator.ProgressValue = porcentagem;
            public async void EncerrarProgresso() => await Barra.ProgressIndicator.HideAsync();
        }

        private class BarraStatusPC
        {
            ApplicationViewTitleBar Barra = ApplicationView.GetForCurrentView().TitleBar;

            public void DefinirCor(Color corDeFundo) => Barra.BackgroundColor = corDeFundo;
            public void DefinirCor(Color corDeFundo, Color corDosBotões)
            {
                DefinirCor(corDeFundo);
                Barra.ButtonBackgroundColor = corDosBotões;
            }

            public static void MostrarBotãoVoltar()
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }
    }
}
