using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Exportacao
    {
        /// <summary>
        /// Sigla da UF de Embarque ou de transposição de fronteira.
        /// </summary>
        [XmlElement("UFSaidaPais", Order = 0)]
        public string UFSaidaPais { get; set; }

        /// <summary>
        /// Descrição do Local de Embarque ou de transposição de fronteira.
        /// </summary>
        [XmlElement("xLocExporta", Order = 1)]
        public string XLocExporta { get; set; }

        /// <summary>
        /// (Opcional)
        /// Descrição do local de despacho.
        /// </summary>
        [XmlElement("xLocDespacho", Order = 2)]
        public string XLocDespacho { get; set; }
    }
}
