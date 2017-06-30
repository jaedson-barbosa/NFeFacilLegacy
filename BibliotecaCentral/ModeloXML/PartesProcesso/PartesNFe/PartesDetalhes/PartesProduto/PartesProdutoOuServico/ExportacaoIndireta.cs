using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class ExportacaoIndireta
    {
        /// <summary>
        /// Número do Registro de Exportação.
        /// </summary>
        [XmlElement("nRE", Order = 0)]
        public long NRE { get; set; }

        /// <summary>
        /// Chave de Acesso da NF-e recebida para exportação;
        /// </summary>
        [XmlElement("chNFe", Order = 1)]
        public string ChNFe { get; set; }

        /// <summary>
        /// Quantidade do item realmente exportado.
        /// </summary>
        [XmlElement("qExport", Order = 2)]
        public double QExport { get; set; }
    }
}
