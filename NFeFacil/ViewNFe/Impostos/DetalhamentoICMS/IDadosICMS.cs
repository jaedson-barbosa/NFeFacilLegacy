using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    interface IDadosICMS
    {
        object Processar(DetalhesProdutos prod);
        //bool Validar(ILog log);
    }
}
