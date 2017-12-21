using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS tributado pela alíquota.
    /// </summary>
    public sealed class COFINSAliq : ComumCOFINS
    {

        [DescricaoPropriedade("Valor da Base de Cálculo da COFINS")]
        [XmlElement(Order = 1)]
        public string vBC { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em percentual)")]
        [XmlElement(Order = 2)]
        public string pCOFINS { get; set; }

        [DescricaoPropriedade("Valor da COFINS")]
        [XmlElement(Order = 3)]
        public string vCOFINS { get; set; }
    }
}
