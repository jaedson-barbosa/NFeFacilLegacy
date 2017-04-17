using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class GrupoExportacao
    {
        /// <summary>
        /// (Opcional)
        /// O número do Ato Concessório de Suspensão deve ser preenchido com 11 dígitos (AAAANNNNNND).
        /// O número do Ato Concessório de Drawback Isenção deve ser preenchido com 9 dígitos (AANNNNNND).
        /// </summary>
        [XmlElement("nDraw")]
        public string NDraw { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo sobre exportação indireta.
        /// </summary>
        [XmlElement("exportInd")]
        public ExportacaoIndireta ExportInd { get; set; }
    }
}
