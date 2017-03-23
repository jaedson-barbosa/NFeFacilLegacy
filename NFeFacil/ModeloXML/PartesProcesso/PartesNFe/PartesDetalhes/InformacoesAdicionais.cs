using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class InformacoesAdicionais
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
        public Observação[] obsCont;

        /// <summary>
        /// (Opcional)
        /// Grupo Processo referenciado.
        /// </summary>
        [XmlElement(nameof(procRef))]
        public ProcessoReferenciado[] procRef;
    }

    public class Observação
    {
        /// <summary>
        /// Identificação do campo.
        /// </summary>
        public string xCampo;

        /// <summary>
        /// Conteúdo do campo.
        /// </summary>
        public string xTexto;
    }

    public class ProcessoReferenciado
    {
        /// <summary>
        /// Identificador do processo ou ato concessório.
        /// </summary>
        public string nProc;

        /// <summary>
        /// Indicador da origem do processo.
        /// </summary>
        public ushort indProc;
    }
}
