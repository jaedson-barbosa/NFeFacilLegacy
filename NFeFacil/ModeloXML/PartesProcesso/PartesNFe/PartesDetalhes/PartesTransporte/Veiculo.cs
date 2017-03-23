using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class Veiculo
    {
        [XmlElement("placa")]
        public string Placa { get; set; }
        public string UF { get; set; }

        /// <summary>
        /// (Opcional)
        /// Registro Nacional de Transportador de Carga.
        /// </summary>
        public string RNTC { get; set; }
    }
}
