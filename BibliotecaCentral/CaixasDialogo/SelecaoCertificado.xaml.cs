using BibliotecaCentral.Certificacao;
using Comum.Primitivos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
