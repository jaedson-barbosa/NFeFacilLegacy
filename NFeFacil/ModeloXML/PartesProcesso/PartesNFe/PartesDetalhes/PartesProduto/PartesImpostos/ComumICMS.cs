using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// O preenchimento dos campos do grupo de ICMS são variáveis e dependem do CST ou do CSOSN do item de Produto.
    /// Por causa do tamanho desta classe será usada uma solução do tipo "gambiarra".
    /// </summary>
    public abstract class ComumICMS
    {
        [DescricaoPropriedade("Origem da mercadoria")]
        [XmlElement("orig", Order = 0)]
        public int Orig { get; set; }
    }
}
