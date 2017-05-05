using Windows.Storage;

namespace BibliotecaCentral.Configuracoes
{
    public sealed class ConfiguracoesCertificacao
    {
        private static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public FonteCertificacao FonteEscolhida
        {
            get => (FonteCertificacao)(Pasta.Values[nameof(FonteEscolhida)] ?? (FonteEscolhida = FonteCertificacao.RepositorioWindows));
            set => Pasta.Values[nameof(FonteEscolhida)] = (int)value;
        }

        public string CertificadoEscolhido
        {
            get => Pasta.Values[nameof(CertificadoEscolhido)] as string;
            set => Pasta.Values[nameof(CertificadoEscolhido)] = value;
        }
    }
}
