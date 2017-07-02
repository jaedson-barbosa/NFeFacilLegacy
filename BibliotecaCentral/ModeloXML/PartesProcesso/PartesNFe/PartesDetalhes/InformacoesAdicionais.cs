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
        [XmlElement("infAdFisco", Order = 0)]
        public string InfAdFisco { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informações Complementares de interesse do Contribuinte.
        /// </summary>
        [XmlElement("infCpl", Order = 1)]
        public string InfCpl { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Campo de uso livre do contribuinte.
        /// </summary>
        [XmlElement("obsCont", Order = 2)]
        public List<Observacao> ObsCont { get; } = new List<Observacao>();

        /// <summary>
        /// (Opcional)
        /// Grupo Processo referenciado.
        /// </summary>
        [XmlElement("procRef", Order = 3)]
        public List<ProcessoReferenciado> ProcRef { get; } = new List<ProcessoReferenciado>();
    }
}
