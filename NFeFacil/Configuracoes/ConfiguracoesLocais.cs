using Windows.Storage;

namespace NFeFacil.Configuracoes
{
    internal static class ConfiguracoesLocais
    {
        internal static readonly ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;
    }
}
