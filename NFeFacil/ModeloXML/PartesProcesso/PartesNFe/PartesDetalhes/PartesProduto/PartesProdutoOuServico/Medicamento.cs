using System;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Medicamento
    {
        /// <summary>
        /// Número do Lote.
        /// </summary>
        public string nLote { get; set; }

        /// <summary>
        /// Quantidade de produtos.
        /// </summary>
        public double qLote { get; set; }

        /// <summary>
        /// Database.Principal de fabricação.
        /// Formato: “AAAA-MM-DD”.
        /// </summary>
        public string dFab;

        [XmlIgnore]
        public DateTimeOffset DFab
        {
            get => dFab != null ? DateTimeOffset.Parse(dFab) : DateTimeOffset.Now;
            set => dFab = value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Database.Principal de validade.
        /// Formato: “AAAA-MM-DD”.
        /// </summary>
        public string dVal;

        [XmlIgnore]
        public DateTimeOffset DVal
        {
            get => dVal != null ? DateTimeOffset.Parse(dVal) : DateTimeOffset.Now;
            set => dVal = value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Preço máximo consumidor.
        /// </summary>
        public double vPMC { get; set; }
    }
}
