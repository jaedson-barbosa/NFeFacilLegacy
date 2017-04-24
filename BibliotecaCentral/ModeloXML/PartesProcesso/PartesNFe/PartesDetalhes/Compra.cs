using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Compra
    {
        /// <summary>
        /// (Opcional)
        /// Nota de Empenho.
        /// </summary>
        [XmlElement("xNEmp")]
        public string XNEmp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Pedido.
        /// </summary>
        [XmlElement("xPed")]
        public string XPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Contrato.
        /// </summary>
        [XmlElement("xCont")]
        public string XCont { get; set; }
    }
}
