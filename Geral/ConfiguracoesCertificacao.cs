namespace BaseGeral
{
    public static class ConfiguracoesCertificacao
    {
        public static OrigemCertificado Origem
        {
            get => (OrigemCertificado)AssistenteConfig.Get(nameof(Origem), 0);
            set => AssistenteConfig.Set(nameof(Origem), (int)value);
        }
    }
}
