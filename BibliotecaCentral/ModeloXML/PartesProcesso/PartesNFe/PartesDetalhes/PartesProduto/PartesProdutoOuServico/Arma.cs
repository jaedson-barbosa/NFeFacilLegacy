namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Arma
    {
        /// <summary>
        /// Indicador do tipo de arma de fogo.
        /// 0=Uso permitido; 1=Uso restrito.
        /// </summary>
        public ushort tpArma { get; set; }

        /// <summary>
        /// Número de série da arma.
        /// </summary>
        public string nSerie { get; set; }

        /// <summary>
        /// Número de série do cano.
        /// </summary>
        public string nCano { get; set; }

        /// <summary>
        /// Descrição completa da arma, compreendendo:
        /// calibre, marca, capacidade, tipo de funcionamento, comprimento e demais elementos que permitam a sua perfeita identificação.
        /// </summary>
        public string descr { get; set; }
    }
}
