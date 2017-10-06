﻿using Comum.Primitivos;
using NFeFacil.Certificacao;
using NFeFacil.Certificacao.LAN;
using NFeFacil.Importacao;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ConfiguracoesCertificado : Page
    {
        public ConfiguracoesCertificado()
        {
            InitializeComponent();
            ListaCertificados = new ObservableCollection<CertificadoExibicao>();
            AttLista();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Permissions, "Certificação");
        }

        ObservableCollection<CertificadoExibicao> ListaCertificados { get; }
        bool InstalacaoLiberada => AnalyticsInfo.VersionInfo.DeviceFamily.Contains("Desktop");

        async void ImportarCertificado(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (await new ImportarCertificado().ImportarEAdicionarAsync())
            {
                AttLista();
            }
        }

        async void ConectarServidor(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await InformacoesConexao.Cadastrar();
        }

        void EsquecerServidor(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            InformacoesConexao.Esquecer();
        }

        async void InstalarServidor(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await new Exportacao(Log.Popup.Current).Exportar("ServidorCertificacao", "Servidor de certificação", "Arquivo comprimido", "zip");
        }

        private void RemoverCertificado(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var serial = ((CertificadoExibicao)contexto).SerialNumber;
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadWrite);
                var cert = loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
                loja.Remove(cert);
            }
            AttLista();
        }

        async void AttLista()
        {
            try
            {
                ListaCertificados.Clear();
                foreach (var item in await Certificados.ObterCertificadosAsync(OrigemCertificado.Importado))
                {
                    ListaCertificados.Add(item);
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }
    }
}
