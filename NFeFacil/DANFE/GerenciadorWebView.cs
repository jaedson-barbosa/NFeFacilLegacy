using NFeFacil.DANFE.Processamento;
using BibliotecaCentral.ModeloXML;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE
{
    public abstract class GerenciadorWebView
    {
        protected string Chave { get; }
        protected ViewDados ExibiçãoDados;
        protected ViewUI UI;

        protected GerenciadorWebView(Processo processo, ref WebView webView)
        {
            var id = processo.NFe.Informações.Id;
            Chave = id.Substring(id.IndexOf('e') + 1);
            ExibiçãoDados = new ViewDados(ref webView, processo);
            UI = new ViewUI(ref webView);
        }

        protected async Task ObterPaginasWeb(Action<int> açaoCadaView)
        {
            var original = UI.ObterDimensoesView();
            for (int i = 0; i < ExibiçãoDados.TotalPaginas; i++)
            {
                await ExibiçãoDados.ExibirUmaPágina(i);
                açaoCadaView?.Invoke(i);
            }
        }
    }
}
