using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class CupomFiscalReferenciado
    {
        /// <summary>
        /// Modelo do Documento Fiscal.
        /// </summary>
        [XmlElement("mod")]
        public string Mod { get; set; }

        /// <summary>
        /// Informar o número de ordem sequencial do ECF que emitiu o Cupom Fiscal vinculado à NF-e.
        /// </summary>
        [XmlElement("nECF")]
        public string NECF { get; set; }

        /// <summary>
        /// Informar o Número do Contador de Ordem de Operação - COO vinculado à NF-e.
        /// </summary>
        [XmlElement("nCOO")]
        public string NCOO { get; set; }
    }
}
