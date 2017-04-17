using Windows.Storage;

namespace BibliotecaCentral.Configuracoes
{
    internal static class ConfiguracoesLocais
    {
        internal static readonly ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;
    }
}
