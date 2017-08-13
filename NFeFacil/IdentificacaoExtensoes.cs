using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using Windows.ApplicationModel;

namespace NFeFacil
{
    static class IdentificacaoExtensoes
    {
        internal static void DefinirVersãoAplicativo(this Identificacao ident)
        {
            var version = Package.Current.Id.Version;
            ident.VersaoAplicativo = $"{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}
