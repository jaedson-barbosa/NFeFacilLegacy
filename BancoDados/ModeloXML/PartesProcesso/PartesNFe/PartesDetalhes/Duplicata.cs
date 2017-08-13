using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Duplicata
    {
        /// <summary>
        /// (Opcional)
        /// Número da Duplicata.
        /// </summary>
        [XmlElement("nDup")]
        public string NDup { get; set; }

        /// <summary>
        /// (Opcional)
        /// Data de vencimento.
        /// Formato: “AAAA-MM-DD” 
        /// </summary>
        [XmlElement("dVenc")]
        public string DVenc { get; set; }

        /// <summary>
        /// Valor da duplicata.
        /// </summary>
        [XmlElement("vDup")]
        public double VDup { get; set; }
    }
}
