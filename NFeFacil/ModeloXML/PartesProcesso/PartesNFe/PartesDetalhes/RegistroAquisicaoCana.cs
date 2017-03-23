using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class RegistroAquisicaoCana
    {
        /// <summary>
        /// Identificação da safra.
        /// Formato: AAAA ou AAAA/AAAA
        /// </summary>
        public string safra { get; set; }

        /// <summary>
        /// Mês e ano de referência.
        /// Formato: MM/AAAA
        /// </summary>
        [XmlElement("ref")]
        public string referencia { get; set; }

        /// <summary>
        /// Quantidade Total do Mês.
        /// </summary>
        public string qTotMes { get; set; }

        /// <summary>
        /// Quantidade Total Anterior.
        /// </summary>
        public string qTotAnt { get; set; }

        /// <summary>
        /// Quantidade Total Geral.
        /// </summary>
        public string qTotGer { get; set; }

        /// <summary>
        /// Valor dos Fornecimentos.
        /// </summary>
        public string vFor { get; set; }

        /// <summary>
        /// Valor Total da Dedução.
        /// </summary>
        public string vTotDed { get; set; }

        /// <summary>
        /// Valor Líquido dos Fornecimentos.
        /// </summary>
        public string vLiqFor { get; set; }

        /// <summary>
        /// Grupo Fornecimento diário de cana.
        /// </summary>
        [XmlElement(nameof(forDia))]
        public List<FornecimentoDiário> forDia= new List<FornecimentoDiário>();

        /// <summary>
        /// (Opcional)
        /// Grupo Deduções – Taxas e Contribuições.
        /// </summary>
        [XmlElement(nameof(deduc))]
        public List<Deduções> deduc = new List<Deduções>();
    }

    public sealed class FornecimentoDiário
    {
        /// <summary>
        /// Dia.
        /// </summary>
        public string dia { get; set; }

        /// <summary>
        /// Quantidade.
        /// </summary>
        public string qtde { get; set; }
    }

    public sealed class Deduções
    {
        /// <summary>
        /// Descrição da Dedução.
        /// </summary>
        public string xDed { get; set; }

        /// <summary>
        /// Valor da Dedução.
        /// </summary>
        public string vDed { get; set; }
    }
}
