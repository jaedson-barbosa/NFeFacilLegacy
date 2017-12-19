using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Compra
    {
        /// <summary>
        /// (Opcional)
        /// Nota de Empenho.
        /// </summary>
        [XmlElement("xNEmp", Order = 0), DescricaoPropriedade("Nota de empenho")]
        public string XNEmp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Pedido.
        /// </summary>
        [XmlElement("xPed", Order = 1), DescricaoPropriedade("Pedido")]
        public string XPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Contrato.
        /// </summary>
        [XmlElement("xCont", Order = 2), DescricaoPropriedade("Contrato")]
        public string XCont { get; set; }
    }
}
