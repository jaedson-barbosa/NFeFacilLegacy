using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class ExportacaoIndireta
    {
        /// <summary>
        /// Número do Registro de Exportação.
        /// </summary>
        [XmlElement("nRE")]
        public ulong NRE { get; set; }

        /// <summary>
        /// Chave de Acesso da NF-e recebida para exportação;
        /// </summary>
        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        /// <summary>
        /// Quantidade do item realmente exportado.
        /// </summary>
        [XmlElement("qExport")]
        public double QExport { get; set; }
    }
}
