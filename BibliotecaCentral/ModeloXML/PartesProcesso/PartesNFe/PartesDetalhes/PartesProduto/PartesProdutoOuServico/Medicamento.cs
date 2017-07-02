using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Medicamento
    {
        /// <summary>
        /// Número do Lote.
        /// </summary>
        [XmlElement("nLote", Order = 0)]
        public string NLote { get; set; }

        /// <summary>
        /// Quantidade de produtos.
        /// </summary>
        [XmlElement("qLote", Order = 1)]
        public double QLote { get; set; }

        /// <summary>
        /// Database.Principal de fabricação.
        /// Formato: “AAAA-MM-DD”.
        /// </summary>
        [XmlElement("dFab", Order = 2)]
        public string DFab { get; set; }

        /// <summary>
        /// Database.Principal de validade.
        /// Formato: “AAAA-MM-DD”.
        /// </summary>
        [XmlElement("dVal", Order = 3)]
        public string DVal { get; set; }

        /// <summary>
        /// Preço máximo consumidor.
        /// </summary>
        [XmlElement("vPMC", Order = 4)]
        public double VPMC { get; set; }
    }
}
