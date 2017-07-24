using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class FornecimentoDiario
    {
        /// <summary>
        /// Dia.
        /// </summary>
        [XmlElement("dia", Order = 0)]
        public int Dia { get; set; }

        /// <summary>
        /// Quantidade.
        /// </summary>
        [XmlElement("qtde", Order = 1)]
        public double Qtde { get; set; }
    }
}
