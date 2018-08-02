using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS60 : ComumICMS, IRegimeNormal
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Tributação do ICMS")]
        public string CST { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Valor da BC do ICMS Retido Anteriormente")]
        public string vBCSTRet { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Alíquota suportada pelo Consumidor Final")]
        public string pST { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor do ICMS Retido Anteriormente")]
        public string vICMSSTRet { get; set; }
    }
}
