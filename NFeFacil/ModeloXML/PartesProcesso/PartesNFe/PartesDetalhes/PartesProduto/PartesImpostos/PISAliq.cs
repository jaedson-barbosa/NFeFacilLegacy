using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado pela alíquota.
    /// </summary>
    public sealed class PISAliq : ComumPIS
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Valor da BC do PIS")]
        public string vBC { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota do PIS (em percentual)")]
        public string pPIS { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor do PIS")]
        public string vPIS { get; set; }
    }
}
