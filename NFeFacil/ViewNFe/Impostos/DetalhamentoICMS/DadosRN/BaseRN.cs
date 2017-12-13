using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    abstract class BaseRN : IDadosICMS
    {
        public string CST { protected get; set; }
        public int Origem { protected get; set; }
        public abstract object Processar(DetalhesProdutos prod);
    }
}
