namespace NFeFacil.Certificacao
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
            get => AssistenteConfig.Get(nameof(Origem), OrigemCertificado.Importado);
            set => AssistenteConfig.Set(nameof(Origem), value);
        }
    }
}
