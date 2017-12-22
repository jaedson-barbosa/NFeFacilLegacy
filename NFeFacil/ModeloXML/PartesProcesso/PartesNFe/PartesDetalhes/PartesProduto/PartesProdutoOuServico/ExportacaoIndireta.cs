using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class ExportacaoIndireta
    {
        [XmlElement("nRE", Order = 0), DescricaoPropriedade("Número do Registro de Exportação")]
        public long NRE { get; set; }

        [XmlElement("chNFe", Order = 1), DescricaoPropriedade("Chave de Acesso da NF-e recebida para exportação")]
        public string ChNFe { get; set; }

        [XmlElement("qExport", Order = 2), DescricaoPropriedade("Quantidade do item realmente exportado")]
        public double QExport { get; set; }
    }
}
