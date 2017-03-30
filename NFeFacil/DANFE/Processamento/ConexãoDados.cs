using Newtonsoft.Json;
using NFeFacil.DANFE.Modelos.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE.Processamento
{
    public sealed class ConexãoDados
    {
        private WebView View { get; }

        public ConexãoDados(ref WebView webView)
        {
            View = webView;
        }

        public async Task AddPrimeiraPage(DadosPrimeiraPagina dados)
        {
            await View.InvokeScriptAsync(
                nameof(AddPrimeiraPage),
                Serializar(dados.cabec, dados.nfe, dados.cliente, dados.motorista, dados.imposto, dados.Produto, dados.extras, dados.duplicatas, dados.paginaTotal));
        }

        public async Task AddOutraPage(DadosOutrasPaginas dados)
        {
            await View.InvokeScriptAsync(nameof(AddOutraPage), Serializar(dados.nfe, dados.cliente, dados.Produto, dados.extras, dados.paginaAtual, dados.paginaTotal));
        }

        public async Task ApagarCorpo()
        {
            await View.InvokeScriptAsync(nameof(ApagarCorpo), null);
            await Task.Delay(100);
        }

        static string Serializar(object obj) => JsonConvert.SerializeObject(obj);

        static IEnumerable<string> Serializar(params object[] objs) => from obj in objs
                                                                              let éStr = obj is string
                                                                              select éStr ? obj as string : Serializar(obj);
    }
}
