using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class NF1AReferenciada
    {
        public string CNPJ { get; set; }

        /// <summary>
        /// Modelo do Documento Fiscal.
        /// </summary>
        [XmlElement("mod")]
        public string Mod { get; set; } = "01";

        /// <summary>
        /// Código da UF do emitente.
        /// </summary>
        [XmlElement("cUF")]
        public ushort CUF { get; set; }

        /// <summary>
        /// Ano e Mês de emissão da NF-e.
        /// </summary>
        public string AAMM { get; set; }

        /// <summary>
        /// Informar zero se não utilizada Série do documento fiscal.
        /// </summary>
        [XmlElement("serie")]
        public uint Serie { get; set; }

        /// <summary>
        /// Número do Documento Fiscal.
        /// </summary>
        [XmlElement("nNF")]
        public string NNF { get; set; }
    }
}
