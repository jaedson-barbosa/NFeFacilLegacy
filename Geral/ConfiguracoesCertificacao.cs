namespace BaseGeral.Certificacao
{
    public static class ConfiguracoesCertificacao
    {
        public static string IPServidorCertificacao
        {
            get => AssistenteConfig.Get<string>(nameof(IPServidorCertificacao), null);
            set => AssistenteConfig.Set(nameof(IPServidorCertificacao), value);
        }

        public static OrigemCertificado Origem
        {
            get => (OrigemCertificado)AssistenteConfig.Get(nameof(Origem), 0);
            set => AssistenteConfig.Set(nameof(Origem), (int)value);
        }
    }
}
