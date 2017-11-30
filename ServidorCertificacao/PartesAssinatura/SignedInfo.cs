namespace ServidorCertificacao.PartesAssinatura
{
    public struct SignedInfo
    {
        public Algoritmo CanonicalizationMethod { get; set; }
        public Algoritmo SignatureMethod { get; set; }

        public Referencia Reference { get; set; }
    }
}
