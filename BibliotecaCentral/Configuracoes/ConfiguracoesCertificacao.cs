using static BibliotecaCentral.Configuracoes.ConfiguracoesLocais;

namespace BibliotecaCentral.Configuracoes
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
