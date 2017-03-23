namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ISSQN : Imposto
    {
        /// <summary>
        /// Informar o Valor da BC do ISSQN.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Informar a Alíquota do ISSQN.
        /// </summary>
        public string vAliq { get; set; }

        /// <summary>
        /// Informar o Valor do ISSQN.
        /// </summary>
        public string vISSQN { get; set; }

        /// <summary>
        /// Informar o código do município de ocorrência do fato gerador do ISSQN.
        /// </summary>
        public string cMunFG { get; set; }

        /// <summary>
        /// Informar Informar o Item da lista de serviços em que se classifica o serviço no padrão ABRASF (Formato: NN.NN).
        /// </summary>
        public string cListServ { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor dedução para redução da Base de Cálculo.
        /// </summary>
        public string vDeducao { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor outras retenções.
        /// </summary>
        public string vOutro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor desconto incondicionado.
        /// </summary>
        public string vDescIncond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor desconto condicionado.
        /// </summary>
        public string vDescCond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Valor retenção ISS.
        /// </summary>
        public string vISSRet { get; set; }

        /// <summary>
        /// Informar Indicador da exigibilidade do ISS.
        /// </summary>
        public string indISS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do serviço prestado dentro do município.
        /// </summary>
        public string cServico { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o código do município de ocorrência do fato gerador do ISSQN.
        /// Informar "9999999" para serviço fora do País. (campo novo)
        /// </summary>
        public string cMun { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do País onde o serviço foi prestado.
        /// Infomar somente se o município da prestação do serviço for "9999". 
        /// </summary>
        public string cPais { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Número do processo judicial ou administrativo de suspensão da exigibilidade.
        /// Informar somente quando declarada a suspensão da exigibilidade do ISSQN.
        /// </summary>
        public string nProcesso { get; set; }

        /// <summary>
        /// Informar Indicador de incentivo Fiscal.
        /// </summary>
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
