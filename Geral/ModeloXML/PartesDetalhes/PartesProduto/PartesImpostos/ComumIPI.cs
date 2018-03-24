﻿using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public abstract class ComumIPI
    {
        [DescricaoPropriedade("Código da situação tributária do IPI")]
        [XmlElement(Order = 0)]
        public string CST { get; set; }
    }
}
