using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMSST : ComumICMS, IRegimeNormal
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Tributação do ICMS")]
        public string CST { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Valor da BC do ICMS retido na UF remetente")]
        public string vBCSTRet { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor do ICMS retido na UF remetente")]
        public string vICMSSTRet { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor da BC do ICMS ST da UF destino")]
        public string vBCSTDest { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Valor do ICMS ST da UF destino")]
        public string vICMSSTDest { get; set; }
    }
}
