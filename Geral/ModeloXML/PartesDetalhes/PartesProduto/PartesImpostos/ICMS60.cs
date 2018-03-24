﻿using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS60 : ComumICMS, IRegimeNormal
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Tributação do ICMS")]
        public string CST { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Valor da BC do ICMS Retido Anteriormente")]
        public string vBCSTRet { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor do ICMS Retido Anteriormente")]
        public string vICMSSTRet { get; set; }
    }
}
