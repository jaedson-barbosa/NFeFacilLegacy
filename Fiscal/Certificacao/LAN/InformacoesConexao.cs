using BaseGeral.Certificacao;
using System.Threading.Tasks;

namespace Fiscal.Certificacao.LAN
{
    public static class InformacoesConexao
    {
        public async static Task<bool> Cadastrar()
        {
            //var caixa = new ConectarServidor();
            //if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            //{
            //    ConfiguracoesCertificacao.IPServidorCertificacao = caixa.IP;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            await Task.Delay(500);
            return true;
        }

        public static void Esquecer()
        {
            ConfiguracoesCertificacao.IPServidorCertificacao = null;
        }
    }
}
