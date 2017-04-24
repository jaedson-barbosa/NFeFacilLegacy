using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class InformacoesAdicionais
    {
        /// <summary>
        /// (Opcional)
        /// Informações Adicionais de Interesse do Fisco.
        /// </summary>
        public string infAdFisco { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informações Complementares de interesse do Contribuinte.
        /// </summary>
        public string infCpl { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Campo de uso livre do contribuinte.
        /// </summary>
        [XmlElement(nameof(obsCont))]
        public Observacao[] obsCont;

        /// <summary>
        /// (Opcional)
        /// Grupo Processo referenciado.
        /// </summary>
        [XmlElement(nameof(procRef))]
        public ProcessoReferenciado[] procRef;
    }
}
