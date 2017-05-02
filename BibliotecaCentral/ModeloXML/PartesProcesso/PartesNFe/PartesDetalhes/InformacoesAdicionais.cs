using System.Collections.Generic;
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
        public List<Observacao> obsCont { get; } = new List<Observacao>();

        /// <summary>
        /// (Opcional)
        /// Grupo Campo de uso livre do Fisco
        /// </summary>
        [XmlElement(nameof(obsFisco))]
        public List<Observacao> obsFisco;

        /// <summary>
        /// (Opcional)
        /// Grupo Processo referenciado.
        /// </summary>
        [XmlElement(nameof(procRef))]
        public List<ProcessoReferenciado> procRef { get; } = new List<ProcessoReferenciado>();
    }
}
