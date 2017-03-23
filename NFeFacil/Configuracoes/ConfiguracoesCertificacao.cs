using static NFeFacil.Configuracoes.ConfiguracoesLocais;

namespace NFeFacil.Configuracoes
{
    public static class ConfiguracoesCertificacao
    {
        public static string Certificado
        {
            get { return Pasta.Values[nameof(Certificado)] as string; }
            set { Pasta.Values[nameof(Certificado)] = value; }
        }
    }
}
