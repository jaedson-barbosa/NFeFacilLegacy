using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Duplicata
    {
        /// <summary>
        /// (Opcional)
        /// Número da Duplicata.
        /// </summary>
        [XmlElement("nDup"), DescricaoPropriedade("Número")]
        public string NDup { get; set; }

        /// <summary>
        /// (Opcional)
        /// Data de vencimento.
        /// Formato: “AAAA-MM-DD” 
        /// </summary>
        [XmlElement("dVenc"), DescricaoPropriedade("Data de vencimento")]
        public string DVenc { get; set; }

        /// <summary>
        /// Valor da duplicata.
        /// </summary>
        [XmlElement("vDup"), DescricaoPropriedade("Valor")]
        public double VDup { get; set; }
    }
}
