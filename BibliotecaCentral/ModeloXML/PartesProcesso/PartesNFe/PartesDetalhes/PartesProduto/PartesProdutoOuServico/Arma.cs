using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Arma
    {
        /// <summary>
        /// Indicador do tipo de arma de fogo.
        /// 0=Uso permitido; 1=Uso restrito.
        /// </summary>
        [XmlElement("tpArma", Order = 0)]
        public byte TpArma { get; set; }

        /// <summary>
        /// Número de série da arma.
        /// </summary>
        [XmlElement("nSerie", Order = 1)]
        public string NSerie { get; set; }

        /// <summary>
        /// Número de série do cano.
        /// </summary>
        [XmlElement("nCano", Order = 2)]
        public string NCano { get; set; }

        /// <summary>
        /// Descrição completa da arma, compreendendo:
        /// calibre, marca, capacidade, tipo de funcionamento, comprimento e demais elementos que permitam a sua perfeita identificação.
        /// </summary>
        [XmlElement("descr", Order = 3)]
        public string Descr { get; set; }
    }
}
