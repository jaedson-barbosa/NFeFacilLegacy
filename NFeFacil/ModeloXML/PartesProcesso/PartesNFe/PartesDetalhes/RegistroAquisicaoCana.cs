using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class RegistroAquisicaoCana
    {
        /// <summary>
        /// Identificação da safra.
        /// Formato: AAAA ou AAAA/AAAA
        /// </summary>
        [XmlElement("safra", Order = 0)]
        public string Safra { get; set; }

        /// <summary>
        /// Mês e ano de referência.
        /// Formato: MM/AAAA
        /// </summary>
        [XmlElement("ref", Order = 1)]
        public string Referencia { get; set; }

        /// <summary>
        /// Grupo Fornecimento diário de cana.
        /// </summary>
        [XmlElement("forDia", Order = 2)]
        public List<FornecimentoDiario> ForDia { get; } = new List<FornecimentoDiario>();

        /// <summary>
        /// Quantidade Total do Mês.
        /// </summary>
        [XmlElement("qTotMes", Order = 3)]
        public double QTotMes { get; set; }

        /// <summary>
        /// Quantidade Total Anterior.
        /// </summary>
        [XmlElement("qTotAnt", Order = 4)]
        public double QTotAnt { get; set; }

        /// <summary>
        /// Quantidade Total Geral.
        /// </summary>
        [XmlElement("qTotGer", Order = 5)]
        public double QTotGer { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Deduções – Taxas e Contribuições.
        /// </summary>
        [XmlElement("deduc", Order = 6)]
        public List<Deducoes> Deduc { get; } = new List<Deducoes>();

        /// <summary>
        /// Valor dos Fornecimentos.
        /// </summary>
        [XmlElement("vFor", Order = 7)]
        public double VFor { get; set; }

        /// <summary>
        /// Valor Total da Dedução.
        /// </summary>
        [XmlElement("vTotDed", Order = 8)]
        public double VTotDed { get; set; }

        /// <summary>
        /// Valor Líquido dos Fornecimentos.
        /// </summary>
        [XmlElement("vLiqFor", Order = 9)]
        public double VLiqFor { get; set; }
    }
}
