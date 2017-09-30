using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo CRT=1 – Simples Nacional e CSOSN=102, 103, 300 ou 400.
    /// </summary>
    public class ICMSSN102 : ComumICMS, ISimplesNacional
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }
    }
}
