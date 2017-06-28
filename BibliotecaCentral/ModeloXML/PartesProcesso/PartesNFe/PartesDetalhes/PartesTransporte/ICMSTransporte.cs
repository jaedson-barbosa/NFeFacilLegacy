namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class ICMSTransporte
    {
        /// <summary>
        /// Valor do Serviço.
        /// </summary>
        public double vServ { get; set; }

        /// <summary>
        /// BC da Retenção do ICMS.
        /// </summary>
        public double vBCRet { get; set; }

        /// <summary>
        /// Alíquota da Retenção.
        /// </summary>
        public double pICMSRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido.
        /// </summary>
        public double vICMSRet { get; set; }

        public long CFOP { get; set; }

        /// <summary>
        /// Código do município de ocorrência do fato gerador do ICMS do transporte.
        /// </summary>
        public int cMunFG { get; set; }
    }
}
