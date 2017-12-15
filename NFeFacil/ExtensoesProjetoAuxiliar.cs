using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.WebService.Pacotes;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace NFeFacil
{
    static class ExtensoesProjetoAuxiliar
    {
        internal static void DefinirVersãoAplicativo(this Identificacao ident)
        {
            var version = Package.Current.Id.Version;
            ident.VersaoAplicativo = $"{version.Major}.{version.Minor}.{version.Build}";
        }

        internal static async Task Assinar(this ProtocoloNFe prot)
        {
            await new AssinaFacil(prot).Assinar<Evento>(prot.InfProt.Id, "infProt");
        }
    }
}
