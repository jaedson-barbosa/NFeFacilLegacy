using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class NF1AReferenciada
    {
        /// <summary>
        /// Código da UF do emitente.
        /// </summary>
        [XmlElement("cUF", Order = 0)]
        public ushort CUF { get; set; }

        /// <summary>
        /// Ano e Mês de emissão da NF-e.
        /// </summary>
        [XmlElement(Order = 1)]
        public string AAMM { get; set; }

        [XmlElement(Order = 2)]
        public long CNPJ { get; set; }

        /// <summary>
        /// Modelo do Documento Fiscal.
        /// </summary>
        [XmlElement("mod", Order = 3)]
        public string Mod { get; set; } = "01";

        /// <summary>
        /// Informar zero se não utilizada Série do documento fiscal.
        /// </summary>
        [XmlElement("serie", Order = 4)]
        public int Serie { get; set; }

        /// <summary>
        /// Número do Documento Fiscal.
        /// </summary>
        [XmlElement("nNF", Order = 5)]
        public int NNF { get; set; }
    }
}
