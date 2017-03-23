namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Exportacao
    {
        /// <summary>
        /// Sigla da UF de Embarque ou de transposição de fronteira.
        /// </summary>
        public string UFSaidaPais { get; set; }

        /// <summary>
        /// Descrição do Local de Embarque ou de transposição de fronteira.
        /// </summary>
        public string xLocExporta { get; set; }

        /// <summary>
        /// (Opcional)
        /// Descrição do local de despacho.
        /// </summary>
        public string xLocDespacho { get; set; }
    }
}
