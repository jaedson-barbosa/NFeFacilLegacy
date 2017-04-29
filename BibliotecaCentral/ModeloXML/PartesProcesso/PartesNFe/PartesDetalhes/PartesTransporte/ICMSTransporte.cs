namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class ICMSTransporte
    {
        /// <summary>
        /// Valor do Serviço.
        /// </summary>
        public string vServ { get; set; }

        /// <summary>
        /// BC da Retenção do ICMS.
        /// </summary>
        public string vBCRet { get; set; }

        /// <summary>
        /// Alíquota da Retenção.
        /// </summary>
        public string pICMSRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido.
        /// </summary>
        public string vICMSRet { get; set; }

        public string CFOP { get; set; }

        /// <summary>
        /// Código do município de ocorrência do fato gerador do ICMS do transporte.
        /// </summary>
        public string cMunFG { get; set; }
    }
}
