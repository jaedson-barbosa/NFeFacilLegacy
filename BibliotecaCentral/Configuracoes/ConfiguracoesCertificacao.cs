using Windows.Storage;

namespace BibliotecaCentral.Configuracoes
{
    public sealed class ConfiguracoesCertificacao
    {
        private static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public FonteCertificacao FonteEscolhida
        {
            get => (FonteCertificacao)(Pasta.Values[nameof(FonteEscolhida)] ?? (FonteEscolhida = FonteCertificacao.RepositorioWindows));
            set => Pasta.Values[nameof(FonteEscolhida)] = value;
        }

        public string SerialEscolhido
        {
            get => Pasta.Values[nameof(SerialEscolhido)] as string;
            set => Pasta.Values[nameof(SerialEscolhido)] = value;
        }
    }
}
