using BaseGeral.View;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes
{
    public sealed class InformacoesAdicionais
    {
        string infAdFisco;
        /// <summary>
        /// (Opcional)
        /// Informações Adicionais de Interesse do Fisco.
        /// </summary>
        [XmlElement("infAdFisco", Order = 0), DescricaoPropriedade("Informações adicionais de interesse do fisco")]
        public string InfAdFisco
        {
            get => string.IsNullOrEmpty(infAdFisco) ? null : infAdFisco;
            set => infAdFisco = value;
        }

        string infCpl;
        /// <summary>
        /// (Opcional)
        /// Informações Complementares de interesse do Contribuinte.
        /// </summary>
        [XmlElement("infCpl", Order = 1), DescricaoPropriedade("Informações complementares de interesse do contribuinte")]
        public string InfCpl
        {
            get => string.IsNullOrEmpty(infCpl) ? null : infCpl;
            set => infCpl = value;
        }

        /// <summary>
        /// (Opcional)
        /// Grupo Processo referenciado.
        /// </summary>
        [XmlElement("procRef", Order = 3)]
        public List<ProcessoReferenciado> ProcRef { get; } = new List<ProcessoReferenciado>();
    }
}
