﻿using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class GrupoExportacao
    {
        [XmlElement("nDraw", Order = 0), DescricaoPropriedade("O número do Ato Concessório")]
        public string NDraw { get; set; }

        [XmlElement("exportInd", Order = 1), DescricaoPropriedade("Grupo sobre exportação indireta")]
        public ExportacaoIndireta ExportInd { get; set; }
    }
}
