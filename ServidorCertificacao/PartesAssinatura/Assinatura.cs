namespace ServidorCertificacao.PartesAssinatura
{
    public sealed class Assinatura
    {
        public SignedInfo SignedInfo { get; set; }
        public string SignatureValue { get; set; }
        public DetalhesChave KeyInfo { get; set; }
    }
}
