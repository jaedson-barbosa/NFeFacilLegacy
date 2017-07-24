using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ISSQN : Imposto
    {
        /// <summary>
        /// Informar o Valor da BC do ISSQN.
        /// </summary>
        [XmlElement(Order = 0)]
        public string vBC { get; set; }

        /// <summary>
        /// Informar a Alíquota do ISSQN.
        /// </summary>
        [XmlElement(Order = 1)]
        public string vAliq { get; set; }

        /// <summary>
        /// Informar o Valor do ISSQN.
        /// </summary>
        [XmlElement(Order = 2)]
        public string vISSQN { get; set; }

        /// <summary>
        /// Informar o código do município de ocorrência do fato gerador do ISSQN.
        /// </summary>
        [XmlElement(Order = 3)]
        public string cMunFG { get; set; }

        /// <summary>
        /// Informar Informar o Item da lista de serviços em que se classifica o serviço no padrão ABRASF (Formato: NN.NN).
        /// </summary>
        [XmlElement(Order = 4)]
        public string cListServ { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor dedução para redução da Base de Cálculo.
        /// </summary>
        [XmlElement(Order = 5)]
        public string vDeducao { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor outras retenções.
        /// </summary>
        [XmlElement(Order = 6)]
        public string vOutro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor desconto incondicionado.
        /// </summary>
        [XmlElement(Order = 7)]
        public string vDescIncond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor desconto condicionado.
        /// </summary>
        [XmlElement(Order = 8)]
        public string vDescCond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor retenção ISS.
        /// </summary>
        [XmlElement(Order = 9)]
        public string vISSRet { get; set; }

        /// <summary>
        /// Informar Indicador da exigibilidade do ISS.
        /// </summary>
        [XmlElement(Order = 10)]
        public string indISS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do serviço prestado dentro do município.
        /// </summary>
        [XmlElement(Order = 11)]
        public string cServico { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o código do município de ocorrência do fato gerador do ISSQN.
        /// Informar "9999999" para serviço fora do País. (campo novo)
        /// </summary>
        [XmlElement(Order = 12)]
        public string cMun { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do País onde o serviço foi prestado.
        /// Infomar somente se o município da prestação do serviço for "9999". 
        /// </summary>
        [XmlElement(Order = 13)]
        public string cPais { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Número do processo judicial ou administrativo de suspensão da exigibilidade.
        /// Informar somente quando declarada a suspensão da exigibilidade do ISSQN.
        /// </summary>
        [XmlElement(Order = 14)]
        public string nProcesso { get; set; }

        /// <summary>
        /// Informar Indicador de incentivo Fiscal.
        /// </summary>
        [XmlElement(Order = 15)]
        public string indIncentivo { get; set; }

        public override bool IsValido
        {
            get
            {
                return NaoNulos(cListServ, cMun, cMunFG, cPais, cServico, indIncentivo, indISS, nProcesso,
                    vAliq, vBC, vDeducao, vDescCond, vDescIncond, vISSQN, vISSRet, vOutro);
            }
        }
    }
}
