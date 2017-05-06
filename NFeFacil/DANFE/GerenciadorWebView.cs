using NFeFacil.DANFE.Pacotes;
using NFeFacil.DANFE.Processamento;
using BibliotecaCentral.ModeloXML;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE
{
    public abstract class GerenciadorWebView
    {
        protected Geral Dados;
        protected ViewDados ExibiçãoDados;
        protected ViewUI UI;

        protected GerenciadorWebView(Processo processo, ref WebView webView)
        {
            Dados = NFeToDANFE.Converter(processo);
            ExibiçãoDados = new ViewDados(ref webView, Dados);
            UI = new ViewUI(ref webView);
        }

        protected async Task ObterPaginasWeb(Func<int, Task> açaoCadaView)
        {
            var original = UI.ObterDimensoesView();
            try
            {
                for (int i = 0; i < ExibiçãoDados.TotalPaginas; i++)
                {
                    await ExibiçãoDados.ExibirUmaPágina(i);
                    await açaoCadaView?.Invoke(i);
                }
            }
            finally
            {
                UI.DefinirDimensoesView(original);
            }
        }
    }
}
