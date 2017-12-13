using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    abstract class BaseSN : IDadosICMS
    {
        public string CSOSN { protected get; set; }
        public int Origem { protected get; set; }
        public abstract object Processar(DetalhesProdutos prod);
    }
}
