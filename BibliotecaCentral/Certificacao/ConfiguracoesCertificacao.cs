using Windows.Storage;

namespace BibliotecaCentral.Certificacao
{
    public sealed class ConfiguracoesCertificacao
    {
        private static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public string CertificadoEscolhido
        {
            get => Pasta.Values[nameof(CertificadoEscolhido)] as string;
            set => Pasta.Values[nameof(CertificadoEscolhido)] = value;
        }
    }
}
