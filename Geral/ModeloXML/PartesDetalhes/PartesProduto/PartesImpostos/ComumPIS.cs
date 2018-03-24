﻿using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS.
    /// </summary>
    public class ComumPIS
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Código de Situação Tributária do PIS")]
        public string CST { get; set; }
    }
}
