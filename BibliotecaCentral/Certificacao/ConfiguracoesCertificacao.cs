using Windows.Storage;

namespace BibliotecaCentral.Certificacao
{
    public static class ConfiguracoesCertificacao
    {
        private static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public static string CertificadoEscolhido
        {
            get => Pasta.Values[nameof(CertificadoEscolhido)] as string;
            set => Pasta.Values[nameof(CertificadoEscolhido)] = value;
        }

        public static string IPServidorCertificacao
        {
            get => Pasta.Values[nameof(IPServidorCertificacao)] as string;
            set => Pasta.Values[nameof(IPServidorCertificacao)] = value;
        }
    }
}
