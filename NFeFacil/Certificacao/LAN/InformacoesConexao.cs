using System;
using System.Threading.Tasks;

namespace NFeFacil.Certificacao.LAN
{
    public static class InformacoesConexao
    {
        public async static Task<bool> Cadastrar()
        {
            var caixa = new ConectarServidor();
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                var ip = caixa.IP;
                ConfiguracoesCertificacao.IPServidorCertificacao = ip;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Esquecer()
        {
            ConfiguracoesCertificacao.IPServidorCertificacao = null;
        }
    }
}
